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
        public static void Nest(ProjectFile item, ProjectFile parentFile)
        {
            if (!string.IsNullOrEmpty(parentFile.FilePath) && item != null)
            {
                item.DependsOn = parentFile.FilePath;
                IdeApp.ProjectOperations.SaveAsync(item.Project.ParentSolution);                
            }
        }

        public static void UnNest(ProjectFile item)
        {
            item.DependsOn = null;
            IdeApp.ProjectOperations.SaveAsync(item.Project);
            //foreach (ProjectItem child in item.ProjectItems)
            //{
            //    UnNest(child);
            //}
        }
    }
}
