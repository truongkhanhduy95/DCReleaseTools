using System;
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
        }

        protected override void Update(CommandInfo info)
        {
            var selectedFile = IdeApp.ProjectOperations.CurrentSelectedItem as ProjectFile;
            info.Enabled = FileHelper.IsXML(selectedFile)

            //info.Enabled = isLayoutFile;
            info.Enabled = false; //temp lock
        }
    }
}
