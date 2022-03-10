using AllGreen.Lib.Core;
using AllGreen.Lib.Core.DomainModel.Script;
using AllGreen.Lib.Core.DomainModel.ScriptResult;

namespace AllGreen.Lib
{
    public abstract class ContextBase<C> : IContext<C> where C : class, IContext, new()
    {
        public ITestScript<C> TestScript { get; set; }

        public ITestScriptResult<C> TestScriptResult { get; set; }

        public abstract void OnTestStart();

        public abstract void OnTestStop();
    }
}