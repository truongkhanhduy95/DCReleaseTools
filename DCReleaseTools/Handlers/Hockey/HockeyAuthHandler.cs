﻿using System;
using MonoDevelop.Components.Commands;

namespace DCReleaseTools.Handlers
{
    public class HockeyAuthHandler : CommandHandler
    {
        public HockeyAuthHandler()
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
