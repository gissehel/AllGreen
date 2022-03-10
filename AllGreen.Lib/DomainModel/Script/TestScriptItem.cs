using AllGreen.Lib.Core;
using AllGreen.Lib.Core.DomainModel.Script;
using System.Collections.Generic;
using System.Linq;

namespace AllGreen.Lib.DomainModel.Script
{
    public abstract class TestScriptItem<C> : PipedNamableScript, ITestScriptItem<C> where C : class, IContext, new()
    {
        public override string GetJson(JsonOptions options) =>
            GetJsonObject(
                GetJsonObjectItems(options).Concat(
                    new List<string>
                    {
                        GetJsonObjectItem("content", GetJsonArray(
                            SubElements == null ? null : SubElements.Select(subElement => subElement.GetJson(options))
                        )),
                    }
                )
            );

        public abstract IEnumerable<IPipedNamableScript> SubElements { get; }
    }
}