using AllGreen.Lib.Core.Fixture;

namespace AllGreen.Lib.Core.Engine.Test
{
    public interface ITestUsingDecoratedSetupFixture<C, F> : ITestUsingSetupFixture<C, F> where C : class, IContext, new() where F : class, IFixture<C>, new()
    {
        ITestMain<C> EndUsing();

        ITestUsingDecoratedSetupFixture<C, F> Enter(params object[] values);
    }
}