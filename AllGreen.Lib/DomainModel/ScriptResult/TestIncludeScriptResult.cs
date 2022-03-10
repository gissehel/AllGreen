using AllGreen.Lib.Core;
using AllGreen.Lib.Core.DomainModel.Pipe;
using AllGreen.Lib.Core.DomainModel.Script;
using AllGreen.Lib.Core.DomainModel.ScriptResult;
using AllGreen.Lib.DomainModel.Pipe;
using AllGreen.Lib.DomainModel.Script;
using System.Collections.Generic;

namespace AllGreen.Lib.DomainModel.ScriptResult
{
    public class TestIncludeScriptResult<C> : TestScriptItemResult, ITestIncludeScriptResult where C : class, IContext, new()
    {
        public TestScriptResult<C> Parent { get; set; }

        public TestIncludeScript<C> TestIncludeScript { get; set; }

        public TestScriptResult<C> TestScriptResult { get; set; }

        public override IEnumerable<ITestCollectionResult> TestCollectionResults
        {
            get
            {
                yield return TestScriptResult;
            }
        }

        public TestIncludeScriptResult(TestScriptResult<C> parent, TestIncludeScript<C> testIncludeScript, TestScriptResult<C> testScriptResult)
        {
            Parent = parent;
            TestIncludeScript = testIncludeScript;
            TestScriptResult = testScriptResult;

            Parent.TestScriptItemResults.Add(this);
        }

        public override IEnumerable<IPipeResultLine> GetPipeResultLines(PipedNameOptions options, int indentLevel)
        {
            yield return new PipeResultLine(indentLevel, new string[] { "Included from", TestIncludeScript.Name });
            if (!options.Compact)
            {
                yield return new PipeResultLine(indentLevel, "");
            }
            yield return new PipeResultLine(indentLevel, "");
            foreach (var pipeResultLine in TestScriptResult.GetPipeResultLines(options, indentLevel + 1))
            {
                yield return pipeResultLine;
            }
            yield return new PipeResultLine(indentLevel, new string[] { "End included" });
            if (!options.Compact)
            {
                yield return new PipeResultLine(indentLevel, "");
            }
            yield return new PipeResultLine(indentLevel, "");
        }

        public override IPipedNamableScript PipedNamableScript => TestIncludeScript;

        public override IEnumerable<IPipedNamableScriptResult> SubElements => TestScriptResult.SubElements;
    }
}