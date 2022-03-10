using AllGreen.Lib.Core.DomainModel.Json;
using AllGreen.Lib.Core.DomainModel.Pipe;
using AllGreen.Lib.Core.DomainModel.Script;
using AllGreen.Lib.DomainModel.Json;
using System.Collections.Generic;
using System.Text;

namespace AllGreen.Lib.DomainModel.Script
{
    public abstract class PipedNamableScript : JsonCapableElement, IPipedNamableScript, IJsonTest
    {
        public string PipedName => GetPipedName(PipedNameOptions.Default);

        public string GetPipedName(PipedNameOptions options)
        {
            var builder = new StringBuilder();
            foreach (var pipeLine in GetPipeLines(options, 0))
            {
                builder.Append(pipeLine.GetPipedName(options));
                builder.Append("\n");
            }
            return builder.ToString();
        }

        public abstract IEnumerable<IPipeLine> GetPipeLines(PipedNameOptions options, int indentLevel);

        public abstract IEnumerable<string> GetJsonObjectItems(JsonOptions options);

        public override string GetJson(JsonOptions options) => GetJsonObject(GetJsonObjectItems(options));
    }
}