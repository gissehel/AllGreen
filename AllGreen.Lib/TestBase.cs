using AllGreen.Lib.Core;
using AllGreen.Lib.Core.Engine.Test;
using AllGreen.Lib.DomainModel.Script;
using AllGreen.Lib.Engine.Test;

namespace AllGreen.Lib
{
    public abstract class TestBase<C> : ITest<C>, ITestScriptSettable<C> where C : class, IContext, new()
    {
        private TestScript<C> TestScript { get; set; }

        TestScript<C> ITestScriptSettable<C>.TestScript
        {
            set
            {
                TestScript = value;
            }
        }

        public abstract void DoTest();

        public ITestMain<C> StartTest()
        {
            return new TestMain<C>(this, new TestScript<C>());
        }

        public TestScript<C> GetTestScript()
        {
            if (TestScript == null)
            {
                DoTest();
            }

            return TestScript;
        }
    }
}