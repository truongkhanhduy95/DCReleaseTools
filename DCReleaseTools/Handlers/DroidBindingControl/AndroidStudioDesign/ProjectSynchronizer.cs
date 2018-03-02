﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using DCReleaseTools.Utils;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;

namespace DCReleaseTools.Handlers
{
    public class ProjectSynchronizer
    {
        public const string ResFolderName = "Resources";

        private ApplicationArguments args;
        private ProgressMonitor _monitor;

        private readonly AndroidProjectTemplateManager _androidProjectTemplateManager = new AndroidProjectTemplateManager();

        private readonly string _xamarinProjectPath;
        private readonly string _projectName;

        private bool _grantedPermissionsToChangeMainProject = false;

        public ProjectSynchronizer(string xamarinProjectPath, string anideExePath, string androidSDKPath = null)
            : this(new ApplicationArguments()
            {
                XamarinProjectPath = xamarinProjectPath,
                AndroidStudioPath = anideExePath,
                AndroidSDKPath = androidSDKPath,
            })
        { }

        public ProjectSynchronizer(ApplicationArguments args)
        {
            this.args = args;

            _xamarinProjectPath = Path.GetDirectoryName(args.XamarinProjectPath);
            _projectName = Path.GetFileNameWithoutExtension(args.XamarinProjectPath);
        }


        public void Sync(string selectedFile = "") //TODO: Open selected file?
        {
            var resFolder = Path.Combine(_xamarinProjectPath, ResFolderName);
            var ideaProjectDir = _androidProjectTemplateManager.CreateProjectFromTemplate(
                xamarinResourcesDir: resFolder,
                sdkPath: args.AndroidSDKPath,
                templatePath: args.CustomTemplatePath
            );

            AppendLog("Created project dir : {0}", ideaProjectDir);

            //if (!string.IsNullOrEmpty(selectedFile))
            //{
            //    arguments += string.Format(" --line 1 \"{0}\"", selectedFile);
            //}

            AppendLog("Opening Android Studio...");
            _monitor = IdeApp.Workbench.ProgressMonitors.GetStatusProgressMonitor("Opening Android Studio...", Stock.StatusSolutionOperation, false, true, false);
            Process p;

            var path = string.Format("{0}{1}", args.AndroidStudioPath, "/Contents/MacOS/studio");
            if (!File.Exists(path))
                path = string.Format("{0}{1}", args.AndroidStudioPath, "/Contents/MacOS/idea");

            p = Process.Start(new ProcessStartInfo(
                path,
                ideaProjectDir.Replace(" ", "\\ ")
            )
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
            });
            ShowSucess("Android Studio opened.");

            //TODO: Clear project after exit android studio
            //p?.WaitForExit();
            //AppendLog("Closed Android Studio, deleting temp project");
            //DeleteProject(ideaProjectDir);
        }

        void DeleteProject(string ideaProjectDir)
        {
            if (Directory.Exists(ideaProjectDir))
                Directory.Delete(ideaProjectDir, true);
        }

        /// <summary>
        /// ANIDE requires those folders and files to be in lower case (and "xml" extension instead of "axml")
        /// </summary>
        public async Task<bool> MakeResourcesSubdirectoriesAndFilesLowercase(Func<Task<bool>> permissionAsker)
        {
            bool madeChanges = false;
            string rootResDir = Path.Combine(_xamarinProjectPath, ResFolderName);

            //we don't need a recursive traversal here since folders in Res directores must not contain subdirectories.
            foreach (var subdir in Directory.GetDirectories(rootResDir))
            {
                madeChanges |= await RenameToLowercase(subdir, permissionAsker);
                foreach (var file in Directory.GetFiles(subdir))
                {
                    madeChanges |= await RenameToLowercase(file, permissionAsker);
                }
            }
            madeChanges |= ChangeAxmlToXmlInCsproj();
            return madeChanges;
        }

        private async Task<bool> RenameToLowercase(string filePath, Func<Task<bool>> permissionAsker)
        {
            var name = Path.GetFileName(filePath);
            bool isFile = !FileHelper.IsFolder(filePath);
            bool shouldRenameAxml = isFile && ".axml".Equals(Path.GetExtension(filePath), StringComparison.InvariantCultureIgnoreCase);

            /*TODO: rename Resource.ResSubfolder.Something to Resource.ResSubfolder.something everywhere in code*/

            if (name.HasUppercaseChars() || shouldRenameAxml)
            {
                if (!_grantedPermissionsToChangeMainProject)
                {
                    //so we noticed that xamarin project has some needed directories in upper case, let's aks user to rename them
                    if (!await permissionAsker())
                    {
                        AppendLog("Operation cancelled by user");
                        throw new OperationCanceledException("Cancelled by user");
                    }
                    _grantedPermissionsToChangeMainProject = true;
                }
                if (shouldRenameAxml)
                {
                    AppendLog("Renaming {0} from axml to xml and to lowercase", name);
                    FileHelper.RenameFileExtensionAndMakeLowercase(filePath, "xml");
                    return true;
                }
                else
                {
                    AppendLog("Renaming {0} to lowercase", name);
                    FileHelper.RenameFileOrFolderToLowercase(filePath);
                    return true;
                }
            }
            return false;
        }

        private bool ChangeAxmlToXmlInCsproj()
        {
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
                    if (value.StartsWith(ResFolderName, StringComparison.InvariantCultureIgnoreCase) && value.EndsWith(".axml", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var newValue = value.Remove(value.Length - "axml".Length, 1); //remove that "a" from axml to become a just xml
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

        protected void AppendLog(string format, params object[] args)
        {
            var log = string.Format(" > " + format, args);
            Console.WriteLine(log);

        }

        protected void ShowSucess(string log)
        {
            _monitor.ReportSuccess(log);
        }
    }
}
