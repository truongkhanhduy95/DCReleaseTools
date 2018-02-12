using System;
using MonoDevelop.Components.Commands;

namespace DCReleaseTools.Handlers.Hockey
{
    public class HockeyBuildHandler : CommandHandler
    {
        public HockeyBuildHandler()
        {
        }

        protected override void Run()
        {
            
        }

        protected override void Update(CommandInfo info)
        {
            info.Enabled = true;
        }
    }
}
