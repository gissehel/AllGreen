using AllGreen.Lib.Core;
using AllGreen.Lib.Core.DomainModel.Pipe;
using AllGreen.Lib.Core.DomainModel.Script;
using AllGreen.Lib.DomainModel.Pipe;
using System.Collections.Generic;
using System.Linq;

namespace AllGreen.Lib.DomainModel.Script
{
    public class TableFixtureLine<C> : PipedNamableScript, ITableFixtureLine where C : class, IContext, new()
    {
        public TestUsingTableFixtureScript<C> Parent { get; private set; }

        public List<object> PropertyValues { get; private set; } = new List<object>();

        public TableFixtureLine(TestUsingTableFixtureScript<C> parent)
        {
            Parent = parent;
        }

        public override IEnumerable<IPipeLine> GetPipeLines(PipedNameOptions options, int indentLevel)
        {
            yield return new PipeLine(indentLevel, PropertyValues.Select(v => v.ToString()));
        }

        public override IEnumerable<string> GetJsonObjectItems(JsonOptions options) => new List<string>
            {
                GetJsonObjectItem("type", GetJsonScalar("fixtureLine")),
                GetJsonObjectItem("values", GetJsonArray(PropertyValues.Select(v => GetJsonScalar(v.ToString())))),
            };
    }
}