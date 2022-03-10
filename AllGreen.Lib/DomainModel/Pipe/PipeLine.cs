using AllGreen.Lib.Core.DomainModel.Pipe;
using System.Collections.Generic;
using System.Text;

namespace AllGreen.Lib.DomainModel.Pipe
{
    public class PipeLine : IPipeLine
    {
        private int _indentLevel = 0;

        public int IndentLevel => _indentLevel;

        public PipeLine(int indentLevel)
        {
            _indentLevel = indentLevel;
        }

        public PipeLine(int indentLevel, string comment) : this(indentLevel)
        {
            Comment = comment;
        }

        public PipeLine(int indentLevel, IEnumerable<string> parts) : this(indentLevel)
        {
            foreach (var part in parts)
            {
                Parts.Add(part);
            }
        }

        public string Comment { get; set; }

        public List<string> Parts { get; } = new List<string>();

        IEnumerable<string> IPipeLine.Parts => Parts;

        public string PipedName => GetPipedName(PipedNameOptions.Default);

        public string GetPipedName(PipedNameOptions options)
        {
            var builder = new StringBuilder();
            if (options.Indent)
            {
                for (int indentLevel = 0; indentLevel < IndentLevel; indentLevel++)
                {
                    builder.Append("    ");
                }
            }
            if (Comment != null)
            {
                builder.Append(Comment);
            }
            else
            {
                builder.Append("| ");
                builder.Append(string.Join(" | ", Parts.ToArray()));
                builder.Append(" |");
            }
            return builder.ToString();
        }
    }
}