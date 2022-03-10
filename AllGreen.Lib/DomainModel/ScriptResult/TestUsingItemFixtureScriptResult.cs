using AllGreen.Lib.Core;
using AllGreen.Lib.Core.DomainModel.Pipe;
using AllGreen.Lib.Core.DomainModel.Script;
using AllGreen.Lib.Core.DomainModel.ScriptResult;
using AllGreen.Lib.DomainModel.Pipe;
using AllGreen.Lib.DomainModel.Script;
using System.Collections.Generic;
using System.Linq;

namespace AllGreen.Lib.DomainModel.ScriptResult
{
    public class TestUsingItemFixtureScriptResult<C> : TestScriptItemResult, ITestUsingFixtureScriptResult where C : class, IContext, new()
    {
        public TestScriptResult<C> Parent { get; private set; }

        public TestUsingItemFixtureScript<C> TestUsingItemFixtureScript { get; private set; }

        public List<FixtureItemResult<C>> FixtureItemResults { get; private set; } = new List<FixtureItemResult<C>>();

        public override IEnumerable<ITestCollectionResult> TestCollectionResults => FixtureItemResults.Cast<ITestCollectionResult>();

        public TestUsingItemFixtureScriptResult(TestScriptResult<C> parent, TestUsingItemFixtureScript<C> testUsingItemFixtureScript)
        {
            Parent = parent;
            TestUsingItemFixtureScript = testUsingItemFixtureScript;
            Parent.TestScriptItemResults.Add(this);
        }

        public override IEnumerable<IPipeResultLine> GetPipeResultLines(PipedNameOptions options, int indentLevel)
        {
            yield return new PipeResultLine(indentLevel, new string[] { "Using", TestUsingItemFixtureScript.FixtureInfo.Name });
            if (!options.Compact)
            {
                yield return new PipeResultLine(indentLevel, "");
            }
            foreach (var fixtureItemResult in FixtureItemResults)
            {
                foreach (var pipeResultLine in fixtureItemResult.GetPipeResultLines(options, indentLevel + 1))
                {
                    yield return pipeResultLine;
                }
            }
            yield return new PipeResultLine(indentLevel, new string[] { "End using" });
            if (!options.Compact)
            {
                yield return new PipeResultLine(indentLevel, "");
            }
            yield return new PipeResultLine(indentLevel, "");
        }

        public override IPipedNamableScript PipedNamableScript => TestUsingItemFixtureScript;

        public override IEnumerable<IPipedNamableScriptResult> SubElements => FixtureItemResults.Cast<IPipedNamableScriptResult>();

    }
}