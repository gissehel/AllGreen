using AllGreen.Lib.Core;
using AllGreen.Lib.Core.DomainModel.Pipe;
using AllGreen.Lib.Core.DomainModel.Script;
using AllGreen.Lib.Core.DomainModel.ScriptResult;
using AllGreen.Lib.DomainModel.Enumeration;
using AllGreen.Lib.DomainModel.Pipe;
using AllGreen.Lib.DomainModel.Script;
using System.Collections.Generic;
using System.Linq;

namespace AllGreen.Lib.DomainModel.ScriptResult
{
    public class TableFixtureLineResult<C> : TestStatableItemResult, ITableFixtureLineResult where C : class, IContext, new()
    {
        public TestUsingTableFixtureScriptResult<C> Parent { get; private set; }

        public TableFixtureLine<C> TableFixtureLine { get; set; }

        public List<object> ActualPropertyValues { get; private set; } = new List<object>();

        public bool IsMissing { get; set; } = false;

        public bool IsSurplus { get; set; } = false;

        public TableFixtureLineResult(TestUsingTableFixtureScriptResult<C> parent, TableFixtureLine<C> tableFixtureLine)
        {
            Parent = parent;
            TableFixtureLine = tableFixtureLine;

            Parent.FixtureItemResults.Add(this);
            switch (parent.TestUsingTableFixtureScript.Kind)
            {
                case FixtureTableKind.Setup:
                    IsTestCode = true;
                    IsTestResult = false;
                    break;

                case FixtureTableKind.List:
                case FixtureTableKind.SubList:
                case FixtureTableKind.SuperList:
                case FixtureTableKind.Set:
                case FixtureTableKind.SubSet:
                case FixtureTableKind.SuperSet:
                case FixtureTableKind.None:
                    IsTestCode = true;
                    IsTestResult = true;
                    break;

                default:
                    IsTestCode = false;
                    IsTestResult = false;
                    break;
            }
        }

        private IEnumerable<string> CompareValues()
        {
            int propertyPosition = 0;
            foreach (var propertyValue in TableFixtureLine.PropertyValues)
            {
                var expectedPropertyValue = propertyValue.ToString();
                var actualPropertyValue = propertyPosition < ActualPropertyValues.Count ? ActualPropertyValues[propertyPosition].ToString() : "";
                if (actualPropertyValue == expectedPropertyValue)
                {
                    yield return actualPropertyValue;
                }
                else
                {
                    yield return string.Format("Expected : [{0}] Actual : [{1}]", expectedPropertyValue, actualPropertyValue);
                }
                propertyPosition++;
            }
        }

        public override IEnumerable<IPipeResultLine> GetPipeResultLines(PipedNameOptions options, int indentLevel)
        {
            if (Parent.TestUsingTableFixtureScript.Kind == FixtureTableKind.Setup)
            {
                foreach (var pipeLine in TableFixtureLine.GetPipeLines(options, indentLevel))
                {
                    var newPipeLine = new PipeLine(indentLevel, pipeLine.Parts);
                    if (FailureShortMessage != null)
                    {
                        newPipeLine.Parts.Add(string.Format("Failure:[{0}]", FailureShortMessage));
                    }
                    yield return new PipeResultLine(indentLevel)
                    {
                        State = State,
                        Exception = Exception,
                        FailureShortMessage = FailureShortMessage,
                        Duration = Duration,
                        PipeLine = newPipeLine,
                    };
                }
            }
            else
            {
                PipeLine newPipeLine = null;

                if (TableFixtureLine != null)
                {
                    if (IsMissing)
                    {
                        newPipeLine = new PipeLine(indentLevel, TableFixtureLine.PropertyValues.Select(v => string.Format("Missing [{0}]", v.ToString())));
                    }
                    else
                    {
                        newPipeLine = new PipeLine(indentLevel, CompareValues());
                    }
                }
                else
                {
                    // if (IsSurplus)
                    {
                        newPipeLine = new PipeLine(indentLevel, ActualPropertyValues.Select(v => string.Format("Surplus [{0}]", v.ToString())));
                    }
                }
                if (FailureShortMessage != null)
                {
                    newPipeLine.Parts.Add(string.Format("Failure:[{0}]", FailureShortMessage));
                }
                yield return new PipeResultLine(indentLevel)
                {
                    State = State,
                    Exception = Exception,
                    FailureShortMessage = FailureShortMessage,
                    Duration = Duration,
                    PipeLine = newPipeLine,
                };
            }
        }
        public override IPipedNamableScript PipedNamableScript => TableFixtureLine;

        public override string GetJsonActualValue() => GetJsonArray(ActualPropertyValues.Select(v => GetJsonScalar(v.ToString())));
    }
}