using AllGreen.Lib.DomainModel.Script;
using AllGreen.Lib.DomainModel.ScriptResult;

namespace AllGreen.Lib.Core.Engine.Service
{
    public interface ITestRunnerService
    {
        TestScriptResult<C> RunTest<C>(TestScript<C> testScript) where C : class, IContext<C>, new();

        TestScriptResult<C> RunTest<C>(TestScript<C> testScript, C context) where C : class, IContext<C>, new();
    }
}