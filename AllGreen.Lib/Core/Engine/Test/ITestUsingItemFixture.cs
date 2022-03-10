using AllGreen.Lib.Core.Fixture;
using System;
using System.Linq.Expressions;

namespace AllGreen.Lib.Core.Engine.Test
{
    public interface ITestUsingItemFixture<C, F> : IFluent where C : class, IContext, new() where F : class, IFixture<C>, new()
    {
        ITestUsingItemFixture<C, F> Comment(string comment);

        ITestUsingItemFixture<C, F> DoAction(Expression<Action<F>> expressionAction);

        ITestUsingItemFixture<C, F> DoCheck(Expression<Func<F, string>> expressionCheck, string result);

        ITestUsingItemFixture<C, F> DoAccept(Expression<Func<F, bool>> expression);

        ITestUsingItemFixture<C, F> DoReject(Expression<Func<F, bool>> expression);

        ITestMain<C> EndUsing();
    }
}