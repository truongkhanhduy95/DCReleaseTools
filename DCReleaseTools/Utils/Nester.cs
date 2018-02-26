using System;
using System.Collections.Generic;
using System.Linq;
using DCReleaseTools.Dialogs;
using MonoDevelop.Ide;
using MonoDevelop.Projects;

namespace DCReleaseTools.Utils
{
    public class Nester
    {
        public static void Nest(IEnumerable<ProjectFile> items, IEnumerable<ProjectFile> siblings)
        {
            GenerateControlDialog selector = null;

            foreach (ProjectFile item in items)
            {
                //string path = item.FilePath;
                //ProjectFile parent = item.Project.GetProjectFile(selector.SelectedFile);
                //if (parent == null) continue;

                //item.DependsOn = parent.FilePath;
                //    bool mayNeedAttributeSet = item.ContainingProject.Kind.Equals(CordovaKind, System.StringComparison.OrdinalIgnoreCase);
                //    if (mayNeedAttributeSet)
                //    {
                //        SetDependentUpon(item, parent.Name);
                //    }
                //    else
                //    {
                //        item.Remove();
                //        parent.ProjectItems.AddFromFile(path);
                //    }
            }

            IdeApp.ProjectOperations.SaveAsync(items.First().Project);
        }

        public static void UnNest(ProjectFile item)
        {
            item.DependsOn = null;
            IdeApp.ProjectOperations.SaveAsync(item.Project);
            //foreach (ProjectItem child in item.ProjectItems)
            //{
            //    UnNest(child);
            //}

            //UnNestItem(item);
        }
    }
}
