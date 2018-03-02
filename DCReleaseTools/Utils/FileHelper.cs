using System;
using MonoDevelop.Projects;

namespace DCReleaseTools.Utils
{
    public class FileHelper
    {
        public static bool IsXML(ProjectFile selectedFile)
        {
            var isLayoutFile = selectedFile.Name.EndsWith(".xml", StringComparison.Ordinal);
            return isLayoutFile;
        }

        public static bool IsCSharpFile(ProjectFile selectedFile)
        {
            var isLayoutFile = selectedFile.Name.EndsWith(".cs", StringComparison.Ordinal);
            return isLayoutFile;
        }
    }
}
