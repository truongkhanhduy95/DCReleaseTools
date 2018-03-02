using System;
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
            var isLayoutFile = selectedFile.Name.EndsWith(".xml", StringComparison.Ordinal);

            //info.Enabled = isLayoutFile;
            info.Enabled = false; //temp lock
        }
    }
}
