using AllGreen.Lib.Core;
using AllGreen.Lib.Core.Fixture;
using System;
using System.Collections.Generic;

namespace AllGreen.Lib
{
    public class FixtureBase<C> : IFixture<C> where C : class, IContext, new()
    {
        public C Context { get; set; }

        public virtual bool OnEnterSetup() => throw new NotImplementedException();

        public virtual IEnumerable<object> OnQuery() => throw new NotImplementedException();
    }
}