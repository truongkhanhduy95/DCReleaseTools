using System;
using System.Diagnostics;
using System.Linq;

namespace DCReleaseTools.Utils
{
    public static class AndroidIDEDetector
    {
        private const string IDE_NAME = "Android Studio";

        public static string TryFindIdePath()
        {
            string path = null;
            try
            {
                Process proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = "/bin/bash";
                proc.StartInfo.Arguments = "-c '/System/Library/Frameworks/CoreServices.framework/Versions/A/Frameworks/LaunchServices.framework/Versions/A/Support/lsregister -dump | grep -i \\'" + ideName + "\\''";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();

                path = proc.StandardOutput.ReadLine(); //TODO: async
                if (path != null && path.Contains(":"))
                {
                    var paths = path.Trim().Split(':');
                    path = paths.Last()?.Trim();
                }
            }
            catch
            {
                Console.WriteLine("cannot find {0}", IDE_NAME);
            }

            //return path;
            return "/Applications/Android Studio.app";
        }
    }
}
