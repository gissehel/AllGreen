using AllGreen.Lib.Core.DomainModel.Pipe;
using AllGreen.Lib.Core.DomainModel.Script;
using AllGreen.Lib.DomainModel.Enumeration;
using AllGreen.Lib.DomainModel.Pipe;
using System;
using System.Collections.Generic;

namespace AllGreen.Lib.DomainModel.ScriptResult
{
    public class TestCommentItemScriptResult : TestStatableItemResult
    {
        private string Comment { get; set; }

        public TestCommentItemScriptResult(string comment)
        {
            State = TestItemState.Comment;
            IsTestCode = false;
            IsTestResult = false;
            Start = DateTime.Now;
            Comment = comment;
        }

        public override IEnumerable<IPipeResultLine> GetPipeResultLines(PipedNameOptions options, int indentLevel)
        {
            foreach (var comment in Comment.Split('\n'))
            {
                yield return new PipeResultLine(indentLevel, comment);
            }
            if (!options.Compact)
            {
                yield return new PipeResultLine(indentLevel, "");
            }
            yield return new PipeResultLine(indentLevel, "");
        }

        public override IPipedNamableScript PipedNamableScript => null;
        public override string GetJsonActualValue() => null;
    }
}