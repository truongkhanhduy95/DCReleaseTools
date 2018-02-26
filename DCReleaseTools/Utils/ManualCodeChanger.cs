using System;
using System.Collections.Generic;
using MonoDevelop.Projects;

namespace DCReleaseTools.Utils
{
    public class ManualCodeChanger
    {
        public static void CreateControlWrapperClass(ProjectFile parentFile, List<AndroidControl> controlList)
        {
            //TODO: Change parent File
            //Get parentFile name
            var parentName = parentFile.Name;

            //Get parentFile namespace
            var nameSpace = GetClassNameSpace(parentFile.FilePath);

            //TODO: Add child file
            //Create new File with StreamWriter, name = parentFile.ui.cs
            //Add lines one by one, with namespace
            //Add control
            //Add file to project

            //TODO: Nest child to parent

            foreach (var x in controlList)
            {
                System.Diagnostics.Debug.WriteLine(x);
            }
        }

        private static string GetClassNameSpace(string filePath)
        {
            var type = typeof(ProjectFile);
            return null;
        }

    }
}
