using AllGreen.Lib.Core;
using AllGreen.Lib.Core.DomainModel.Pipe;
using AllGreen.Lib.Core.DomainModel.Script;
using AllGreen.Lib.Core.DomainModel.ScriptResult;
using AllGreen.Lib.DomainModel.Script;
using System;
using System.Collections.Generic;

namespace AllGreen.Lib.DomainModel.ScriptResult
{
    public class TestCommentScriptResult<C> : TestScriptItemResult, ITestCommentScriptResult where C : class, IContext, new()
    {
        private TestCommentScript<C> TestCommentScript { get; set; }

        public TestCommentScriptResult(TestScriptResult<C> parent, TestCommentScript<C> testCommentScript)
        {
            TestCommentScript = testCommentScript;

            parent.TestScriptItemResults.Add(this);

            TestCommentItemScriptResult = new TestCommentItemScriptResult(Comment)
            {
                Stop = DateTime.Now
            };
        }

        private TestCommentItemScriptResult TestCommentItemScriptResult { get; set; }

        public override IEnumerable<ITestCollectionResult> TestCollectionResults
        {
            get
            {
                yield return TestCommentItemScriptResult;
            }
        }

        public string Comment => TestCommentScript.Comment;

        public override IEnumerable<IPipeResultLine> GetPipeResultLines(PipedNameOptions options, int indentLevel) => TestCommentItemScriptResult.GetPipeResultLines(options, indentLevel);

        public override IPipedNamableScript PipedNamableScript => TestCommentScript;
        public override IEnumerable<IPipedNamableScriptResult> SubElements => new List<IPipedNamableScriptResult> { };
    }
}