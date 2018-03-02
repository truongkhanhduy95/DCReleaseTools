using System;
namespace DCReleaseTools
{
    public class FileRenameToLowercaseException : Exception
    {
        public string FileName { get; set; }

        public FileRenameToLowercaseException(string fileName, Exception actualException)
            : base("Unable to rename " + fileName + " to lowercase", actualException)
        {
            FileName = fileName;
        }
    }
}
