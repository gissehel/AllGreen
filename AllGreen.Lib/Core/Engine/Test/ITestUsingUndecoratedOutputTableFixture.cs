using AllGreen.Lib.Core.Fixture;
using System;
using System.Linq.Expressions;

namespace AllGreen.Lib.Core.Engine.Test
{
    public interface ITestUsingUndecoratedOutputTableFixture<C, F> : ITestUsingOutputTableFixture<C, F> where C : class, IContext, new() where F : class, IFixture<C>, new()
    {
        ITestUsingDecoratedOutputTableFixture<C, F> With(params string[] names);

        ITestUsingDecoratedOutputTableFixture<C, F> With<R>(params Expression<Func<R, string>>[] expressions);
    }
}