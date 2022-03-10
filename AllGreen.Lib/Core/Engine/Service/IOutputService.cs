using AllGreen.Lib.DomainModel.ScriptResult;
using System;
using System.Collections.Generic;
using System.Text;

namespace AllGreen.Lib.Core.Engine.Service
{
    public interface IOutputService
    {
        void Output<C>(TestScriptResult<C> testScriptsResult, string path, string name) where C : class, IContext<C>, new();
    }
}
