using AllGreen.Lib.Core;
using AllGreen.Lib.Core.Engine.Service;
using AllGreen.Lib.DomainModel;
using AllGreen.Lib.DomainModel.ScriptResult;
using System.IO;
using System.Text;

namespace AllGreen.Lib.Engine.Service
{
    public class TextOutputService : IOutputService
    {
        public void Output<C>(TestScriptResult<C> testScriptResult, string path, string name) where C : class, IContext<C>, new()
        {
            var fullPath = Path.Combine(path, string.Format("{0}.agout", testScriptResult.TestScript.Name));
            using (var writer = new StreamWriter(fullPath, false, new UTF8Encoding(false)))
            {
                writer.Write(testScriptResult.GetPipedName(PipedNameOptions.Detailled));
            }
        }
    }
}
