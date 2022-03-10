using AllGreen.Lib.Core.DomainModel.Json;
using AllGreen.Lib.Core.DomainModel.Pipe;
using AllGreen.Lib.Core.DomainModel.Script;
using AllGreen.Lib.Core.DomainModel.ScriptResult;
using AllGreen.Lib.DomainModel.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllGreen.Lib.DomainModel.ScriptResult
{
    public abstract class PipedNamableScriptResult : JsonCapableElement, IPipedNamableScriptResult, IJsonTestResult
    {
        public string PipedName => GetPipedName(PipedNameOptions.Default);

        public string GetPipedName(PipedNameOptions options)
        {
            var builder = new StringBuilder();
            foreach (var pipeResultLine in GetPipeResultLines(options, 0))
            {
                builder.Append(pipeResultLine.GetPipedName(options));
                builder.Append("\n");
                foreach (var exceptionPipedName in pipeResultLine.GetExceptionPipedNames(options))
                {
                    builder.Append(exceptionPipedName);
                    builder.Append("\n");
                }
            }
            return builder.ToString();
        }

        public abstract IEnumerable<IPipeResultLine> GetPipeResultLines(PipedNameOptions options, int indentLevel);

        public sealed override string GetJson(JsonOptions options) => GetJsonObject(
            GetJsonObjectItems(options)
        );

        public abstract string GetJsonResult(JsonOptions options);

        public abstract IPipedNamableScript PipedNamableScript { get; }

        public IEnumerable<string> GetJsonObjectTestItems(JsonOptions options) => PipedNamableScript != null ? PipedNamableScript.GetJsonObjectItems(options) : new List<string>();

        public virtual IEnumerable<string> GetJsonObjectItems(JsonOptions options) =>
            GetJsonObjectTestItems(options).Concat(
                new List<string> {
                    GetJsonObjectItem("results", GetJsonResult(options)),
                }
            );

    }
}