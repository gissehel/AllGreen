using AllGreen.Lib.Core;
using AllGreen.Lib.Core.DomainModel.Pipe;
using AllGreen.Lib.Core.DomainModel.Script;
using AllGreen.Lib.Core.Fixture;
using AllGreen.Lib.DomainModel.Enumeration;
using AllGreen.Lib.DomainModel.Pipe;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AllGreen.Lib.DomainModel.Script
{
    public class FixtureItem<C> : PipedNamableScript, IFixtureItem where C : class, IContext, new()
    {
        public FixtureItem(FixtureItemKind kind, string name)
        {
            Kind = kind;
            if (kind == FixtureItemKind.Comment)
            {
                Comment = name;
            }
            else
            {
                CodeName = name;
            }
        }

        public FixtureItemKind Kind { get; private set; }

        public Action<IFixture<C>, List<object>> ActionCode { get; set; }
        public Func<IFixture<C>, List<object>, string> CheckCode { get; set; }
        public Func<IFixture<C>, List<object>, bool> AcceptRejectCode { get; set; }

        public string ExpectedCheckResult { get; set; }
        public bool ExpectedAcceptRejectResult { get; set; }

        public string CodeName { get; private set; }

        public string Comment { get; set; }

        public IPipeLine GetPipeLine(PipedNameOptions options, int indentLevel)
        {
            if (Kind == FixtureItemKind.Comment)
            {
                return null;
            }
            else
            {
                var pipeLine = new PipeLine(indentLevel);
                pipeLine.Parts.Add(Kind.ToString());

                var nameParts = CodeName.Split(new string[] { "__" }, StringSplitOptions.None);
                int index = 0;
                foreach (var namePart in nameParts)
                {
                    if (namePart.Length > 0)
                    {
                        pipeLine.Parts.Add(namePart.Replace("_", " "));
                    }
                    if (index < Args.Count)
                    {
                        pipeLine.Parts.Add(Args[index].ToString());
                    }
                    index++;
                }
                if (Kind == FixtureItemKind.Check)
                {
                    pipeLine.Parts.Add(ExpectedCheckResult);
                }
                return pipeLine;
            }
        }

        public override IEnumerable<IPipeLine> GetPipeLines(PipedNameOptions options, int indentLevel)
        {
            if (Kind == FixtureItemKind.Comment)
            {
                if (options.Compact)
                {
                    yield return new PipeLine(indentLevel, "");
                }
                foreach (var comment in Comment.Split('\n'))
                {
                    yield return new PipeLine(indentLevel) { Comment = comment };
                }
                yield return new PipeLine(indentLevel, "");
            }
            else
            {
                yield return GetPipeLine(options, indentLevel);
                if (!options.Compact)
                {
                    yield return new PipeLine(indentLevel, "");
                }
            }
        }

        public List<object> Args { get; } = new List<object>();

        public List<Type> ArgTypes { get; } = new List<Type>();

        public string GetJsonState()
        {
            switch (Kind)
            {
                case FixtureItemKind.Check: return "check";
                case FixtureItemKind.Accept: return "accept";
                case FixtureItemKind.Reject: return "reject";
                case FixtureItemKind.Action: return "action";
                case FixtureItemKind.Comment: return "comment";
                default: return "unknown";
            }
        }

        public string GetJsonName() => Kind == FixtureItemKind.Comment ? null : CodeName;
        public IEnumerable<string> GetJsonNames() => Kind == FixtureItemKind.Comment ? null :
            CodeName
            .Split(new string[] { "__" }, StringSplitOptions.None)
            .Where(namePart => namePart.Length > 0)
            .Select(namePart => namePart.Replace("_", " "))
            .Select(namePart => GetJsonScalarOrNull(namePart))
            ;

        public IEnumerable<string> GetJsonValues() => Kind == FixtureItemKind.Comment ? null : Args.Select(arg => GetJsonScalarOrNull(arg.ToString()));
        public string GetJsonExpectedValue() => Kind == FixtureItemKind.Check ? ExpectedCheckResult : null;

        public string GetJsonComment() => Kind == FixtureItemKind.Comment ? Comment : null;

        public override IEnumerable<string> GetJsonObjectItems(JsonOptions options) => new List<string>
            {
                GetJsonObjectItem("type", GetJsonScalar(GetJsonState())),
                GetJsonObjectItemOrNull("name", GetJsonScalarOrNull(GetJsonName())),
                GetJsonObjectItemOrNull("names", GetJsonArray(GetJsonNames())),
                GetJsonObjectItemOrNull("values", GetJsonArray(GetJsonValues())),
                GetJsonObjectItemOrNull("comment", GetJsonScalarOrNull(GetJsonComment())),
                GetJsonObjectItemOrNull("expected", GetJsonScalarOrNull(GetJsonExpectedValue())),
            };
    }
}