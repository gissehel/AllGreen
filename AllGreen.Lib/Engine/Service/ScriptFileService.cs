using AllGreen.Lib.Core;
using AllGreen.Lib.Core.Engine.Service;
using AllGreen.Lib.DomainModel;
using AllGreen.Lib.DomainModel.Script;
using System.IO;
using System.Text;

namespace AllGreen.Lib.Engine.Service
{
    public class ScriptFileService : IScriptFileService
    {
        public void GenerateScriptFile<C>(TestScript<C> testScript, string filename) where C : class, IContext<C>, new()
        {
            using (var writer = new StreamWriter(filename, false, new UTF8Encoding(false)))
            {
                writer.Write(testScript.GetPipedName(PipedNameOptions.Canonical));
            }
        }
    }
}