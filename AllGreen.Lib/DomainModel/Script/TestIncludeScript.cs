using AllGreen.Lib.Core;
using AllGreen.Lib.Core.DomainModel.Pipe;
using AllGreen.Lib.Core.DomainModel.Script;
using AllGreen.Lib.DomainModel.Pipe;
using System.Collections.Generic;
using System.Linq;

namespace AllGreen.Lib.DomainModel.Script
{
    public class TestIncludeScript<C> : TestScriptItem<C>, ITestIncludeScript where C : class, IContext, new()
    {
        public TestInfo TestInfo { get; private set; }

        public TestScript<C> TestScript { get; private set; }

        public TestIncludeScript(TestInfo testInfo, TestScript<C> testScript)
        {
            TestInfo = testInfo;
            TestScript = testScript;
        }

        public string Name => TestInfo.Name;

        public string SlugName => Name.Replace(" ", "_");

        public string ClassName => SlugName + "_test";

        IEnumerable<ITestScriptItem> INamedTestScript.TestScriptItems => TestScript.TestScriptItems.Cast<ITestScriptItem>();

        public override IEnumerable<IPipeLine> GetPipeLines(PipedNameOptions options, int indentLevel)
        {
            if (options.ForceDisplayIncludedScripts)
            {
                yield return new PipeLine(indentLevel, new string[] { "Included from", Name });
                if (!options.Compact)
                {
                    yield return new PipeLine(indentLevel, "");
                }
                yield return new PipeLine(indentLevel, "");
                foreach (var pipeLine in TestScript.GetPipeLines(options, indentLevel + 1))
                {
                    yield return pipeLine;
                }
                yield return new PipeLine(indentLevel, new string[] { "End included" });
                if (!options.Compact)
                {
                    yield return new PipeLine(indentLevel, "");
                }
                yield return new PipeLine(indentLevel, "");
            }
            else
            {
                yield return new PipeLine(indentLevel, new string[] { "Include", Name });
                if (!options.Compact)
                {
                    yield return new PipeLine(indentLevel, "");
                }
                yield return new PipeLine(indentLevel, "");
            }
        }

        public override IEnumerable<string> GetJsonObjectItems(JsonOptions options) => new List<string>
            {
                GetJsonObjectItem("type", GetJsonScalar("include")),
                GetJsonObjectItem("name", GetJsonScalar(Name)),
            };

        public override IEnumerable<IPipedNamableScript> SubElements => TestScript.TestScriptItems.Cast<IPipedNamableScript>();
    }
}