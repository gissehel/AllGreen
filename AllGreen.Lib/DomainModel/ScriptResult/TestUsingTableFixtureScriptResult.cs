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
    public class TestUsingTableFixtureScriptResult<C> : TestScriptItemResult, ITestUsingTableFixtureScriptResult where C : class, IContext, new()
    {
        public TestScriptResult<C> Parent { get; private set; }

        public TestUsingTableFixtureScript<C> TestUsingTableFixtureScript { get; private set; }

        public TestUsingOutputTableQueryScriptResult<C> TestUsingOutputTableQueryScriptResult { get; private set; }

        public List<TableFixtureLineResult<C>> FixtureItemResults { get; private set; } = new List<TableFixtureLineResult<C>>();

        public override IEnumerable<ITestCollectionResult> TestCollectionResults => FixtureItemResults.Cast<ITestCollectionResult>();

        public TestUsingTableFixtureScriptResult(TestScriptResult<C> parent, TestUsingTableFixtureScript<C> testUsingTableFixtureScript)
        {
            Parent = parent;
            TestUsingTableFixtureScript = testUsingTableFixtureScript;
            TestUsingOutputTableQueryScriptResult = new TestUsingOutputTableQueryScriptResult<C>(this);
            Parent.TestScriptItemResults.Add(this);
        }

        public override IEnumerable<IPipeResultLine> GetPipeResultLines(PipedNameOptions options, int indentLevel)
        {
            foreach (var pipeResultLine in TestUsingOutputTableQueryScriptResult.GetPipeResultLines(options, indentLevel))
            {
                yield return pipeResultLine;
            }
            yield return new PipeResultLine(indentLevel + 1, TestUsingTableFixtureScript.FixturePropertyInfos.Select(f => f.GetPipedName(options)));
            foreach (var fixtureItemResult in FixtureItemResults)
            {
                foreach (var pipeResultLine in fixtureItemResult.GetPipeResultLines(options, indentLevel + 1))
                {
                    yield return pipeResultLine;
                }
            }
            if (!options.Compact)
            {
                yield return new PipeResultLine(indentLevel, "");
            }
            yield return new PipeResultLine(indentLevel, "");
        }

        public override IPipedNamableScript PipedNamableScript => TestUsingTableFixtureScript;

        public override IEnumerable<IPipedNamableScriptResult> SubElements => FixtureItemResults.Cast<IPipedNamableScriptResult>();
    }
}