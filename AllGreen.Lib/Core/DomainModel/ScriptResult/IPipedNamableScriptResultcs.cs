using AllGreen.Lib.Core.DomainModel.Json;
using AllGreen.Lib.Core.DomainModel.Pipe;
using AllGreen.Lib.DomainModel;
using System.Collections.Generic;

namespace AllGreen.Lib.Core.DomainModel.ScriptResult
{
    public interface IPipedNamableScriptResult : IJsonTestResult, IPipedNamable
    {
        IEnumerable<IPipeResultLine> GetPipeResultLines(PipedNameOptions options, int indentLevel);

        IEnumerable<string> GetJsonObjectItems(JsonOptions options);
    }
}