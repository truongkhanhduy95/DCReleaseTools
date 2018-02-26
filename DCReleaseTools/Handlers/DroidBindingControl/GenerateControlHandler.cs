using System;
using DCReleaseTools.Dialogs;
using DCReleaseTools.Utils;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Projects;

namespace DCReleaseTools.Handlers
{
    public class GenarateControlHandler : CommandHandler
    {
        public GenarateControlHandler()
        {
        }

        protected override void Run()
        {
            var parentFile = IdeApp.ProjectOperations.CurrentSelectedItem as ProjectFile;
            GenerateControlDialog selector = null;
            using (selector = new GenerateControlDialog())
            {
                if (!selector.ShowWithParent())
                {
                    return;
                }
            }

            var reader = new ResourceReader();
            reader.LoadFromResource(selector.SelectedFile);

            ManualCodeChanger.CreateControlWrapperClass(parentFile, reader.Controls);
        }

        protected override void Update(CommandInfo info)
        {
            info.Enabled = true;
        }
    }
}
