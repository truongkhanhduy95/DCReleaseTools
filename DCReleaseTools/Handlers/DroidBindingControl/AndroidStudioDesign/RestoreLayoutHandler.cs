using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using DCReleaseTools.Utils;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using MonoDevelop.Projects;

namespace DCReleaseTools.Handlers
{
    public class RestoreLayoutHandler : CommandHandler
    {
        public const string ResFolderName = "Resources";

		protected override void Run()
		{
            RenameToAxml();
            ChangeXmlToAxmlInCsproj();
            IdeApp.ProjectOperations.SaveAsync(IdeApp.ProjectOperations.CurrentSelectedSolution);
		}

        private void RenameToAxml()
        {
            var path = IdeApp.ProjectOperations.CurrentSelectedProject.FileName;
            var _xamarinProjectPath = Path.GetDirectoryName(path);

            string rootResDir = Path.Combine(_xamarinProjectPath, ResFolderName);
            foreach (var subdir in Directory.GetDirectories(rootResDir))
            {
                foreach (var file in Directory.GetFiles(subdir))
                {
                    FileHelper.RenameFileExtensionAndMakeLowercase(file, "axml");        
                }
            }
        }

        private bool ChangeXmlToAxmlInCsproj()
        {
            var path = IdeApp.ProjectOperations.CurrentSelectedProject.FileName;
            var _xamarinProjectPath = Path.GetDirectoryName(path);
            var _projectName = Path.GetFileNameWithoutExtension(path);

            var csProjPath = Path.Combine(_xamarinProjectPath, _projectName + ".csproj");
            var doc = XDocument.Load(csProjPath);

            var androidResNodes = doc.Descendants().Where(n => n.Name.LocalName == "AndroidResource").ToArray();
            bool changed = false;
            foreach (var androidResNode in androidResNodes)
            {
                var includeAttr = androidResNode.Attribute("Include");
                if (includeAttr != null)
                {
                    var value = includeAttr.Value ?? "";
                    if (value.StartsWith(ResFolderName, StringComparison.InvariantCultureIgnoreCase) && value.EndsWith(".xml", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var newValue = value.Insert(value.Length - "xml".Length, "a"); //add that "a" to become axml
                        includeAttr.SetValue(newValue);
                        changed = true;
                    }
                }
            }
            if (changed)
            {
                try
                {
                    doc.Save(csProjPath);
                }
                catch (Exception exc)
                {
                    throw new CsprojEditFailedException(csProjPath, exc);
                }
            }
            return changed;
        }


		protected override void Update(CommandInfo info)
		{
            var selectedFile = IdeApp.ProjectOperations.CurrentSelectedItem as ProjectFile;
            info.Enabled = FileHelper.IsXMLFile(selectedFile);
		}
	}
}
