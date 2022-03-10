using AllGreen.Lib.Core;
using AllGreen.Lib.Core.DomainModel.Pipe;
using AllGreen.Lib.Core.DomainModel.Script;
using AllGreen.Lib.Core.DomainModel.ScriptResult;
using AllGreen.Lib.DomainModel.Pipe;
using System.Collections.Generic;
using System.Linq;

namespace AllGreen.Lib.DomainModel.Script
{
    public class TestUsingItemFixtureScript<C> : TestUsingFixtureScript<C>, ITestUsingItemFixtureScript where C : class, IContext, new()
    {
        public TestUsingItemFixtureScript(TestScript<C> parent, FixtureInfo fixtureInfo)
            : base(parent, fixtureInfo)
        {
        }

        public List<FixtureItem<C>> FixtureItems { get; } = new List<FixtureItem<C>>();

        public override IEnumerable<IPipeLine> GetPipeLines(PipedNameOptions options, int indentLevel)
        {
            yield return new PipeLine(indentLevel, new string[] { "Using", FixtureInfo.Name });
            if (!options.Compact)
            {
                yield return new PipeLine(indentLevel, "");
            }
            foreach (var fixtureItem in FixtureItems)
            {
                foreach (var pipeLine in fixtureItem.GetPipeLines(options, indentLevel + 1))
                {
                    yield return pipeLine;
                }
            }
            yield return new PipeLine(indentLevel, new string[] { "End using" });
            if (!options.Compact)
            {
                yield return new PipeLine(indentLevel, "");
            }
            yield return new PipeLine(indentLevel, "");
        }

        public override IEnumerable<string> GetJsonObjectItems(JsonOptions options) => new List<string>
            {
                GetJsonObjectItem("type", GetJsonScalar("using")),
                GetJsonObjectItem("fixture", GetJsonScalar(FixtureInfo.Name)),
            };

        public override IEnumerable<IPipedNamableScript> SubElements => FixtureItems.Cast<IPipedNamableScript>();
    }
}