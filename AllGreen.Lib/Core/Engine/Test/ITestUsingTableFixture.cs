using AllGreen.Lib.Core.Fixture;

namespace AllGreen.Lib.Core.Engine.Test
{
    public interface ITestUsingTableFixture<C, F> : IFluent where C : class, IContext, new() where F : class, IFixture<C>, new()
    {
    }
}