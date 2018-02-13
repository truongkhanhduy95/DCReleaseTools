using System;
using DCReleaseTools.Dialogs;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;

namespace DCReleaseTools.Handlers
{
    public class GenarateControlHandler : CommandHandler
    {
        public GenarateControlHandler()
        {
           
        }

        protected override void Run()
        {
            var dialog = new GenerateControlDialog();
            MessageService.ShowCustomDialog(dialog);
        }

        protected override void Update(CommandInfo info)
        {
            info.Enabled = true;
        }
    }
}
