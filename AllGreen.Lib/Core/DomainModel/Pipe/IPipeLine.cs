using System.Collections.Generic;

namespace AllGreen.Lib.Core.DomainModel.Pipe
{
    public interface IPipeLine : IPipedNamable
    {
        int IndentLevel { get; }

        string Comment { get; }

        IEnumerable<string> Parts { get; }
    }
}