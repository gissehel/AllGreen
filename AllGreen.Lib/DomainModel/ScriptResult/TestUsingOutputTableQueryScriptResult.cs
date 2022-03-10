using AllGreen.Lib.Core;
using AllGreen.Lib.Core.DomainModel.Pipe;
using AllGreen.Lib.Core.DomainModel.Script;
using AllGreen.Lib.DomainModel.Enumeration;
using AllGreen.Lib.DomainModel.Pipe;
using System;
using System.Collections.Generic;

namespace AllGreen.Lib.DomainModel.ScriptResult
{
    public class TestUsingOutputTableQueryScriptResult<C> : TestStatableItemResult where C : class, IContext, new()
    {
        public TestUsingTableFixtureScriptResult<C> Parent { get; set; }

        public TestUsingOutputTableQueryScriptResult(TestUsingTableFixtureScriptResult<C> parent)
        {
            Parent = parent;
            IsTestCode = true;
            IsTestResult = false;
            Start = DateTime.Now;
            State = TestItemState.None;
        }

        public override IEnumerable<IPipeResultLine> GetPipeResultLines(PipedNameOptions options, int indentLevel)
        {
            yield return new PipeResultLine(indentLevel, new string[] { Parent.TestUsingTableFixtureScript.KindAsString, Parent.TestUsingTableFixtureScript.Name })
            {
                State = State,
                Exception = Exception,
                FailureShortMessage = FailureShortMessage,
                Duration = Duration,
            };
        }
        public override IPipedNamableScript PipedNamableScript => null;

        public override string GetJsonActualValue() => null;
    }
}