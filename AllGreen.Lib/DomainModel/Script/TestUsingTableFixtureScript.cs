using AllGreen.Lib.Core;
using AllGreen.Lib.Core.DomainModel.Pipe;
using AllGreen.Lib.Core.DomainModel.Script;
using AllGreen.Lib.DomainModel.Enumeration;
using AllGreen.Lib.DomainModel.Pipe;
using System.Collections.Generic;
using System.Linq;

namespace AllGreen.Lib.DomainModel.Script
{
    public class TestUsingTableFixtureScript<C> : TestUsingFixtureScript<C>, ITestUsingTableFixtureScript where C : class, IContext, new()
    {
        public TestUsingTableFixtureScript(TestScript<C> parent, FixtureInfo fixtureInfo, FixtureTableKind kind)
            : base(parent, fixtureInfo)
        {
            Kind = kind;
        }

        public FixtureTableKind Kind { get; private set; }

        public List<FixturePropertyInfo<C>> FixturePropertyInfos { get; private set; } = new List<FixturePropertyInfo<C>>();

        public List<TableFixtureLine<C>> Lines { get; private set; } = new List<TableFixtureLine<C>>();

        public override IEnumerable<IPipeLine> GetPipeLines(PipedNameOptions options, int indentLevel)
        {
            yield return new PipeLine(indentLevel, new string[] { KindAsString, Name });
            yield return new PipeLine(indentLevel + 1, FixturePropertyInfos.Select(f => f.GetPipedName(options)));
            foreach (var line in Lines)
            {
                foreach (var pipeLine in line.GetPipeLines(options, indentLevel + 1))
                {
                    yield return pipeLine;
                }
            }
            if (!options.Compact)
            {
                yield return new PipeLine(indentLevel, "");
            }
            yield return new PipeLine(indentLevel, "");
        }

        public string Name => FixtureInfo.Name;

        public string KindAsString
        {
            get
            {
                switch (Kind)
                {
                    case FixtureTableKind.Setup: return "Setup";
                    case FixtureTableKind.List: return "List";
                    case FixtureTableKind.SubList: return "Sub list";
                    case FixtureTableKind.SuperList: return "Super list";
                    case FixtureTableKind.Set: return "Set";
                    case FixtureTableKind.SubSet: return "Sub set";
                    case FixtureTableKind.SuperSet: return "Super set";
                    case FixtureTableKind.None: return "None";
                    default: return "";
                }
            }
        }

        public string KindAsJsonString
        {
            get
            {
                switch (Kind)
                {
                    case FixtureTableKind.Setup: return "setup";
                    case FixtureTableKind.List: return "list";
                    case FixtureTableKind.SubList: return "sublist";
                    case FixtureTableKind.SuperList: return "superList";
                    case FixtureTableKind.Set: return "set";
                    case FixtureTableKind.SubSet: return "subSet";
                    case FixtureTableKind.SuperSet: return "superSet";
                    case FixtureTableKind.None: return "none";
                    default: return "none";
                }
            }
        }

        public override IEnumerable<string> GetJsonObjectItems(JsonOptions options) => new List<string>
            {
                GetJsonObjectItem("type", GetJsonScalar(KindAsJsonString)),
                GetJsonObjectItem("fixture", GetJsonScalar(FixtureInfo.Name)),
                GetJsonObjectItem("properties", GetJsonArray(
                    FixturePropertyInfos.Select(fixturePropertyInfo => GetJsonObject(
                        new List<string>
                        {
                            GetJsonObjectItem("type", GetJsonScalar("propertyInfo")),
                            GetJsonObjectItem("name", GetJsonScalar(fixturePropertyInfo.Name)),
                        }
                    ))
                )),
            };

        public override IEnumerable<IPipedNamableScript> SubElements => Lines.Cast<IPipedNamableScript>();

    }
}