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
    public class TestScriptResult<C> : TestScriptItemResult, ITestScriptResult<C> where C : class, IContext, new()
    {
        public TestScript<C> TestScript { get; private set; }

        public C Context { get; set; }

        public List<PipedNamableScriptResult> TestScriptItemResults { get; private set; } = new List<PipedNamableScriptResult>();

        public override IEnumerable<ITestCollectionResult> TestCollectionResults => TestScriptItemResults.Cast<ITestCollectionResult>();

        public TestScriptResult(TestScript<C> testScript)
        {
            TestScript = testScript;
        }

        public override IEnumerable<IPipeResultLine> GetPipeResultLines(PipedNameOptions options, int indentLevel)
        {
            if (options.DecorateScriptProperties)
            {
                bool hasYield = false;
                if (TestScript.IsRunnable)
                {
                    yield return new PipeResultLine(indentLevel, new string[] { "Runnable" });
                    hasYield = true;
                }

                if (hasYield)
                {
                    if (!options.Compact)
                    {
                        yield return new PipeResultLine(indentLevel, "");
                    }
                    yield return new PipeResultLine(indentLevel, "");
                }
            }
            foreach (var testScriptItemResult in TestScriptItemResults)
            {
                foreach (var pipeResultLine in testScriptItemResult.GetPipeResultLines(options, indentLevel))
                {
                    yield return pipeResultLine;
                }
            }
        }

        public override IPipedNamableScript PipedNamableScript => TestScript;
        public override IEnumerable<IPipedNamableScriptResult> SubElements => TestScriptItemResults.Cast<IPipedNamableScriptResult>();
    }
}