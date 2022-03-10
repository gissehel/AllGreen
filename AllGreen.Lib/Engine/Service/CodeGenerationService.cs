using AllGreen.Lib.Core;
using AllGreen.Lib.Core.Engine.Service;
using AllGreen.Lib.DomainModel.Script;
using AllGreen.Lib.Engine.Service.CodeGeneration;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AllGreen.Lib.Engine.Service
{
    public class CodeGenerationService : ICodeGenerationService
    {
        public void GenerateCode<C>(TestScript<C> testScript, string filename, string namespaceValue) where C : class, IContext<C>, new()
        {
            using (var writer = new StreamWriter(filename, false, new UTF8Encoding(true)))
            {
                var codeGenerator = new CodeGenerator<C>(writer, namespaceValue);
                codeGenerator.GenerateCode(testScript);
            }
        }

        public void GenerateCode<C>(IEnumerable<TestScript<C>> testScripts, string filename, string namespaceValue) where C : class, IContext<C>, new()
        {
            using (var writer = new StreamWriter(filename, false, new UTF8Encoding(true)))
            {
                var codeGenerator = new CodeGenerator<C>(writer, namespaceValue);
                codeGenerator.GenerateCode(testScripts);
            }
        }
    }
}