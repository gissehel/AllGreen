using AllGreen.Lib.Core.DomainModel.Script;
using AllGreen.Lib.Core.DomainModel.ScriptResult;

namespace AllGreen.Lib.Core
{
    public interface IContext
    {
        void OnTestStart();

        void OnTestStop();
    }

    public interface IContext<C> : IContext where C : class, IContext, new()
    {
        ITestScript<C> TestScript { get; set; }

        ITestScriptResult<C> TestScriptResult { get; set; }
    }
}