using AllGreen.Lib.Core.Fixture;

namespace AllGreen.Lib.Core.Engine.Test
{
    public interface ITestUsingDecoratedOutputTableFixture<C, F> : ITestUsingOutputTableFixture<C, F> where C : class, IContext, new() where F : class, IFixture<C>, new()
    {
        ITestMain<C> EndUsing();

        ITestUsingDecoratedOutputTableFixture<C, F> Check(params object[] values);
    }
}