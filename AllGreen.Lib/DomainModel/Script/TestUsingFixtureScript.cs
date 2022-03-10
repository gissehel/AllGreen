using AllGreen.Lib.Core;
using AllGreen.Lib.Core.DomainModel.Script;

namespace AllGreen.Lib.DomainModel.Script
{
    public abstract class TestUsingFixtureScript<C> : TestScriptItem<C>, ITestUsingFixtureScript where C : class, IContext, new()
    {
        public TestScript<C> Parent { get; private set; }

        public FixtureInfo FixtureInfo { get; private set; }

        public TestUsingFixtureScript(TestScript<C> parent, FixtureInfo fixtureInfo)
        {
            Parent = parent;
            FixtureInfo = fixtureInfo;
            Parent.TestScriptItems.Add(this);
        }
    }
}