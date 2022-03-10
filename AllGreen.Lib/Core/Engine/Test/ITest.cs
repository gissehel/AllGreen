using AllGreen.Lib.DomainModel.Script;

namespace AllGreen.Lib.Core.Engine.Test
{
    public interface ITest<C> : IFluent where C : class, IContext, new()
    {
        TestScript<C> GetTestScript();

        ITestMain<C> StartTest();

        void DoTest();
    }
}