using AllGreen.Lib.DomainModel;
using AllGreen.Lib.DomainModel.Enumeration;
using System;
using System.Collections.Generic;

namespace AllGreen.Lib.Core.DomainModel.Pipe
{
    public interface IPipeResultLine : IPipedNamable
    {
        IPipeLine PipeLine { get; }

        TestItemState State { get; }

        TimeSpan? Duration { get; }

        string StateAsString { get; }

        string DurationAsString { get; }

        Exception Exception { get; }

        IEnumerable<string> GetExceptionPipedNames(PipedNameOptions options);
    }
}