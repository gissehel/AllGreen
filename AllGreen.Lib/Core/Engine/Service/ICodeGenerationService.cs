using AllGreen.Lib.DomainModel.Script;
using System.Collections.Generic;

namespace AllGreen.Lib.Core.Engine.Service
{
    public interface ICodeGenerationService
    {
        void GenerateCode<C>(TestScript<C> testScript, string filename, string namespaceValue) where C : class, IContext<C>, new();

        void GenerateCode<C>(IEnumerable<TestScript<C>> testScripts, string filename, string namespaceValue) where C : class, IContext<C>, new();
    }
}