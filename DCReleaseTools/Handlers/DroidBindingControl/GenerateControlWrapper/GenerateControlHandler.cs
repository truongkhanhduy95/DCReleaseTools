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
            var progressMonitor = IdeApp.Workbench.ProgressMonitors.GetStatusProgressMonitor("Generating designer file...", Stock.StatusSolutionOperation, false, true, false);

            try
            {
                var parentFile = IdeApp.ProjectOperations.CurrentSelectedItem as ProjectFile;
                GenerateControlDialog selector = null;
                using (selector = new GenerateControlDialog())
                {
                    if (!selector.ShowWithParent())
                        return;
                }

                var reader = new ResourceReader();
                reader.LoadFromResource(selector.SelectedFile);

                ManualCodeChanger.CreateControlWrapperClass(parentFile, reader.Controls);
            }
            catch
            {
                progressMonitor.ReportError("Cannot generate file!");
                progressMonitor.Dispose();
            }
            finally
            {
                progressMonitor.ReportSuccess("Designer file added!");
                progressMonitor.Dispose();
            }

        }

        protected override void Update(CommandInfo info)
        {
            //TODO: check .cs file or in Droid project
            var selectedFile = IdeApp.ProjectOperations.CurrentSelectedItem as ProjectFile;
            info.Enabled = FileHelper.IsCSharpFile(selectedFile);
        }
    }
}
