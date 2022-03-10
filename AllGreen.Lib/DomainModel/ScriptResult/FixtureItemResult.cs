using AllGreen.Lib.Core;
using AllGreen.Lib.Core.DomainModel.Pipe;
using AllGreen.Lib.Core.DomainModel.Script;
using AllGreen.Lib.Core.DomainModel.ScriptResult;
using AllGreen.Lib.DomainModel.Enumeration;
using AllGreen.Lib.DomainModel.Pipe;
using AllGreen.Lib.DomainModel.Script;
using System.Collections.Generic;

namespace AllGreen.Lib.DomainModel.ScriptResult
{
    public class FixtureItemResult<C> : TestStatableItemResult, IFixtureItemResult where C : class, IContext, new()
    {
        public TestUsingItemFixtureScriptResult<C> Parent { get; private set; }
        public FixtureItem<C> FixtureItem { get; private set; }

        public string ActualResult { get; set; } = null;

        public FixtureItemResult(TestUsingItemFixtureScriptResult<C> parent, FixtureItem<C> fixtureItem)
        {
            Parent = parent;
            FixtureItem = fixtureItem;
            Parent.FixtureItemResults.Add(this);
            switch (FixtureItem.Kind)
            {
                case FixtureItemKind.Check:
                case FixtureItemKind.Accept:
                case FixtureItemKind.Reject:
                    IsTestCode = true;
                    IsTestResult = true;
                    break;

                case FixtureItemKind.Action:
                    IsTestCode = true;
                    IsTestResult = false;
                    break;

                case FixtureItemKind.Comment:
                default:
                    IsTestCode = false;
                    IsTestResult = false;
                    break;
            }
        }

        public override IEnumerable<IPipeResultLine> GetPipeResultLines(PipedNameOptions options, int indentLevel)
        {
            if (State == TestItemState.Comment)
            {
                foreach (var pipeLine in FixtureItem.GetPipeLines(options, indentLevel))
                {
                    yield return new PipeResultLine(indentLevel)
                    {
                        State = State,
                        Exception = Exception,
                        FailureShortMessage = FailureShortMessage,
                        Duration = Duration,
                        PipeLine = pipeLine,
                    };
                }
            }
            else
            {
                var pipeLine = new PipeLine(indentLevel);
                foreach (var part in FixtureItem.GetPipeLine(options, indentLevel).Parts)
                {
                    pipeLine.Parts.Add(part);
                }
                if (State == TestItemState.Error)
                {
                    pipeLine.Parts.Add(string.Format("Actual result:[{0}]", ActualResult));
                }
                if (FailureShortMessage != null)
                {
                    pipeLine.Parts.Add(string.Format("Failure:[{0}]", FailureShortMessage));
                }

                yield return new PipeResultLine(indentLevel)
                {
                    State = State,
                    Exception = Exception,
                    FailureShortMessage = FailureShortMessage,
                    Duration = Duration,
                    PipeLine = pipeLine,
                };
                if (!options.Compact)
                {
                    yield return new PipeResultLine(indentLevel, "");
                }
            }
        }

        public override IPipedNamableScript PipedNamableScript => FixtureItem;

        public override string GetJsonActualValue() => GetJsonScalar(ActualResult);
    }
}