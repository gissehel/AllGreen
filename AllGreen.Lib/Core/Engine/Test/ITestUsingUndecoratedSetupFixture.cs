using AllGreen.Lib.Core.Fixture;
using System;
using System.Linq.Expressions;

namespace AllGreen.Lib.Core.Engine.Test
{
    public interface ITestUsingUndecoratedSetupFixture<C, F> : ITestUsingSetupFixture<C, F> where C : class, IContext, new() where F : class, IFixture<C>, new()
    {
        ITestUsingDecoratedSetupFixture<C, F> With(params Expression<Func<F, object>>[] expressions);
    }
}