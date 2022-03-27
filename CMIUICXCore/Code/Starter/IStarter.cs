using System;
using System.Collections.Generic;

namespace CMIUICXCore.Code.Starter
{
    public interface IStarter
    {
        PlatformID Platform { get; }
        StarterArgumensResult ProcessCommandArgumens(ICollection<string> args);
        StarterRunResult ProcessHostRun();
        void ShowHelp();
    }
}
