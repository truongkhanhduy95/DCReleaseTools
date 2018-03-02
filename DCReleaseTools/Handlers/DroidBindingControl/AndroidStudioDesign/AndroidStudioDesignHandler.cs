using System;
using System.Threading.Tasks;
using DCReleaseTools.Utils;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using MonoDevelop.Projects;

namespace DCReleaseTools.Handlers
{
    public class AndroidStudioDesignHandler : CommandHandler
    {
        protected override void Run()
        {
            //TODO: Command will change resource structure
            //All .axml file will be changed to .xml file to be read in Android Studio
            //Consider to reset files to .xml extension

            var path = IdeApp.ProjectOperations.CurrentSelectedProject.FileName;
            var idePath = AndroidIDEDetector.TryFindIdePath();

            var arg = new ApplicationArguments();
            arg.AndroidSDKPath = string.Empty;
            arg.AndroidStudioPath = idePath;
            arg.CustomTemplatePath = "/Users/bja/Workspaces/Xamarin/Xamaridea/CustomTemplate"; //TODO:Implement custom template
            arg.XamarinProjectPath = path;

            RunAsync(arg);
        }

        public async Task RunAsync(ApplicationArguments args)
        {
            try
            {
                var projectsSynchronizer = new ProjectSynchronizer(args);
                await projectsSynchronizer.MakeResourcesSubdirectoriesAndFilesLowercase(async () => 
                {
                    System.Console.WriteLine("Permissions to change original project has been requested and granted.");
                    return true;
                });
                projectsSynchronizer.Sync();
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
        }

        protected override void Update(CommandInfo info)
        {
            var selectedFile = IdeApp.ProjectOperations.CurrentSelectedItem as ProjectFile;

            info.Enabled = FileHelper.IsResource(selectedFile);
            //info.Enabled = false; //temp lock
        }
    }
}
