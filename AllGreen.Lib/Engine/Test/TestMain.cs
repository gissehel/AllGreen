using AllGreen.Lib.Core;
using AllGreen.Lib.Core.Engine.Test;
using AllGreen.Lib.Core.Fixture;
using AllGreen.Lib.DomainModel;
using AllGreen.Lib.DomainModel.Enumeration;
using AllGreen.Lib.DomainModel.Script;

namespace AllGreen.Lib.Engine.Test
{
    public class TestMain<C> : ITestMain<C> where C : class, IContext, new()
    {
        private ITestScriptSettable<C> Parent { get; set; }

        public TestScript<C> TestScript { get; private set; }

        public ITestMain<C> IsRunnable()
        {
            TestScript.IsRunnable = true;
            return this;
        }

        public TestMain(ITestScriptSettable<C> parent, TestScript<C> testScript)
        {
            Parent = parent;
            TestScript = testScript;
            var testInfo = new TestInfo(Parent.GetType());
            TestScript.Name = testInfo.Name;
        }

        public ITestUsingItemFixture<C, F> Using<F>() where F : class, IFixture<C>, new()
        {
            return new TestUsingItemFixture<C, F>(this, TestScript);
        }

        public ITestUsingUndecoratedSetupFixture<C, F> UsingSetup<F>() where F : class, IFixture<C>, new()
        {
            return new TestUsingTableFixture<C, F>(this, TestScript, FixtureTableKind.Setup);
        }

        public ITestUsingUndecoratedOutputTableFixture<C, F> UsingList<F>() where F : class, IFixture<C>, new()
        {
            return new TestUsingTableFixture<C, F>(this, TestScript, FixtureTableKind.List);
        }

        public ITestMain<C> Comment(string comment)
        {
            TestScript.TestScriptItems.Add(new TestCommentScript<C>(comment));
            return this;
        }

        public ITestMain<C> Include<T>() where T : class, ITest<C>, new()
        {
            var test = new T();
            var testInfo = new TestInfo(typeof(T));
            var testIncludeScript = new TestIncludeScript<C>(testInfo, test.GetTestScript());
            TestScript.TestScriptItems.Add(testIncludeScript);
            return this;
        }

        public void EndTest()
        {
            Parent.TestScript = TestScript;
        }
    }
}