using AllGreen.Lib.Core.Engine.Service;
using AllGreen.Lib.DomainModel;
using AllGreen.Lib.DomainModel.ScriptResult;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AllGreen.Lib.Engine.Service
{
    public class JsonOutputService : IOutputService
    {
        void IOutputService.Output<C>(TestScriptResult<C> testScriptResult, string path, string name)
        {
            var fullPath = Path.Combine(path, string.Format("{0}.agout.json", testScriptResult.TestScript.Name));
            using (var writer = new StreamWriter(fullPath, false, new UTF8Encoding(false)))
            {
                writer.Write(testScriptResult.GetJson(JsonOptions.Default));
            }
        }
    }
}
