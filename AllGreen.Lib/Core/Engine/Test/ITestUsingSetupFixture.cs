using AllGreen.Lib.Core.Fixture;

namespace AllGreen.Lib.Core.Engine.Test
{
    public interface ITestUsingSetupFixture<C, F> : ITestUsingTableFixture<C, F> where C : class, IContext, new() where F : class, IFixture<C>, new()
    {
    }
}