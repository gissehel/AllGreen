using AllGreen.Lib.Core.DomainModel.Json;
using AllGreen.Lib.Core.DomainModel.Pipe;
using AllGreen.Lib.DomainModel;
using System.Collections.Generic;

namespace AllGreen.Lib.Core.DomainModel.Script
{
    public interface IPipedNamableScript : IJsonTest, IPipedNamable
    {
        IEnumerable<IPipeLine> GetPipeLines(PipedNameOptions options, int indentLevel);

        IEnumerable<string> GetJsonObjectItems(JsonOptions options);
    }
}