using AllGreen.Lib.Core;
using AllGreen.Lib.Core.DomainModel.Pipe;
using AllGreen.Lib.Core.DomainModel.Script;
using AllGreen.Lib.DomainModel.Pipe;
using System.Collections.Generic;

namespace AllGreen.Lib.DomainModel.Script
{
    public class TestCommentScript<C> : TestScriptItem<C>, ITestCommentScript where C : class, IContext, new()
    {
        public string Comment { get; private set; }

        public TestCommentScript(string comment)
        {
            Comment = comment;
        }

        public override IEnumerable<IPipeLine> GetPipeLines(PipedNameOptions options, int indentLevel)
        {
            foreach (var comment in Comment.Split('\n'))
            {
                yield return new PipeLine(indentLevel, comment);
            }
            if (!options.Compact)
            {
                yield return new PipeLine(indentLevel, "");
            }
            yield return new PipeLine(indentLevel, "");
        }

        public override IEnumerable<string> GetJsonObjectItems(JsonOptions options) => new List<string>
            {
                GetJsonObjectItem("type", GetJsonScalar("comment")),
                GetJsonObjectItem("comment", GetJsonScalar(Comment)),
            };

        public override IEnumerable<IPipedNamableScript> SubElements => null;

    }
}