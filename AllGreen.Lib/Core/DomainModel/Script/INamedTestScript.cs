using System.Collections.Generic;

namespace AllGreen.Lib.Core.DomainModel.Script
{
    public interface INamedTestScript : IPipedNamableScript
    {
        string Name { get; }

        string SlugName { get; }

        string ClassName { get; }

        IEnumerable<ITestScriptItem> TestScriptItems { get; }
    }
}