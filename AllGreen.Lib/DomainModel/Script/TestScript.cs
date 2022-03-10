using AllGreen.Lib.Core;
using AllGreen.Lib.Core.DomainModel.Pipe;
using AllGreen.Lib.Core.DomainModel.Script;
using AllGreen.Lib.DomainModel.Pipe;
using System.Collections.Generic;
using System.Linq;

namespace AllGreen.Lib.DomainModel.Script
{
    public class TestScript<C> : PipedNamableScript, ITestScript<C> where C : class, IContext, new()
    {
        public string Name { get; set; }

        public string SlugName => Name.Replace(" ", "_");

        public string ClassName => SlugName + "_test";

        public List<TestScriptItem<C>> TestScriptItems { get; } = new List<TestScriptItem<C>>();

        IEnumerable<ITestScriptItem> INamedTestScript.TestScriptItems => TestScriptItems.Cast<ITestScriptItem>();

        /// <summary>
        /// True if the test is runnable. Otherwise, it will only be availlable for other tests to include.
        /// </summary>
        public bool IsRunnable { get; set; } = false;

        public override IEnumerable<IPipeLine> GetPipeLines(PipedNameOptions options, int indentLevel)
        {
            if (options.DecorateScriptProperties)
            {
                bool hasYield = false;
                if (IsRunnable)
                {
                    yield return new PipeLine(indentLevel, new string[] { "Runnable" });
                    hasYield = true;
                }

                if (hasYield)
                {
                    if (!options.Compact)
                    {
                        yield return new PipeLine(indentLevel, "");
                    }
                    yield return new PipeLine(indentLevel, "");
                }
            }
            foreach (var testUsingFixtureScript in TestScriptItems)
            {
                foreach (var pipeLine in testUsingFixtureScript.GetPipeLines(options, indentLevel))
                {
                    yield return pipeLine;
                }
            }
        }
        public override IEnumerable<string> GetJsonObjectItems(JsonOptions options) => new List<string>
            {
                GetJsonObjectItem("type", GetJsonScalar("test")),
                GetJsonObjectItem("name", GetJsonScalar(Name)),
                GetJsonObjectItem("isRunnable", GetJsonScalar(IsRunnable)),
            };
    }
}