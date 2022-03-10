using AllGreen.Lib.Core.Engine.Test;
using System.Collections.Generic;

namespace AllGreen.Lib.Core.Fixture
{
    public interface IFixture<C> : IFluent where C : IContext
    {
        C Context { get; set; }

        bool OnEnterSetup();

        IEnumerable<object> OnQuery();
    }
}