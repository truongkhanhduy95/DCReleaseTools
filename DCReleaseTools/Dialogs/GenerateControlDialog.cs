using System;
using System.Collections.Generic;
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

        public GenerateControlDialog(IEnumerable<ProjectFile> items, IEnumerable<ProjectFile> siblings)
        {
            Build();
            okButton.Clicked += OkButton_Clicked;

            ProjectFile currentItem = items.First();

            files = GetSource(siblings, items, new Dictionary<string, string>(), string.Empty);
            foreach (string key in files.Keys)
            {
                filesComboBox.Items.Add(key);
            }

            int index = GetMatchParentIndex(currentItem.Name, files.Keys.ToArray());
            filesComboBox.SelectedIndex = index;
        }

        internal bool ShowWithParent()
        {
            WindowFrame parent = Toolkit.CurrentEngine.WrapWindow(IdeApp.Workbench.RootWindow);
            return Run(parent) == Xwt.Command.Ok;
        }

        private int GetMatchParentIndex(string currentFileName, string[] allFiles)
        {
            int maxFileIndex = 0;
            int maxFileEqualCharCount = 0;

            for (int i = 0; i < allFiles.Length; i++)
            {
                var fileNameInList = allFiles[i];
                var count = GetEqualCharCount(currentFileName, fileNameInList);
                if (count <= maxFileEqualCharCount) continue;
                maxFileEqualCharCount = count;
                maxFileIndex = i;
            }
            return maxFileIndex;
        }

        int GetEqualCharCount(string currentFileName, string fileNameInList)
        {
            return fileNameInList.TakeWhile((t, i) => i < currentFileName.Length).TakeWhile((t, i) => currentFileName[i] == t).Count();
        }

        IDictionary<string, string> GetSource(IEnumerable<ProjectFile> parents, IEnumerable<ProjectFile> selected, Dictionary<string, string> paths, string indentation)
        {
            foreach (ProjectFile item in parents)
            {
                if (!selected.Contains(item))
                {
                    string path = indentation + item.FilePath.FileName;

                    if (!paths.ContainsKey(path))
                        paths.Add(path, item.FilePath);
                }

                GetSource(item.DependentChildren, selected, paths, indentation + "    ");
            }

            return paths;
        }

        void OkButton_Clicked(object sender, EventArgs e)
        {
            SelectedFile = files[filesComboBox.SelectedItem.ToString()];
        }
    }
}
