using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MonoDevelop.Ide;
using MonoDevelop.Projects;
using Xwt;

namespace DCReleaseTools.Dialogs
{
    public partial class GenerateControlDialog
    {
        public string SelectedFile { get; private set; }

        IDictionary<string, string> files;

        public GenerateControlDialog(){
            Build();
            files = GetLayoutResource(); 
            foreach (string key in files.Keys)
            {
                filesComboBox.Items.Add(key);
            }
            filesComboBox.SelectedIndex = 0;
        }

        internal bool ShowWithParent()
        {
            WindowFrame parent = Toolkit.CurrentEngine.WrapWindow(IdeApp.Workbench.RootWindow);
            return Run(parent) == Xwt.Command.Ok;
        }

        private IDictionary<string, string> GetLayoutResource()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            var projectPath = IdeApp.ProjectOperations.CurrentSelectedProject.FileName.FullPath.ParentDirectory.ToString();
            var directories = Path.Combine(projectPath, "Resources/layout");

            foreach (var file in Directory.GetFiles(directories))
            {
                dic.Add(Path.GetFileName(file), file);
            }
            return dic;
        }

        void OkButton_Clicked(object sender, EventArgs e)
        {
            SelectedFile = files[filesComboBox.SelectedItem.ToString()];
        }
    }
}
