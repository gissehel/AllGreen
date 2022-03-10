namespace AllGreen.Lib.Core.DomainModel.ScriptResult
{
    public interface ITestScriptResult<C> : ITestScriptItemResult where C : class, IContext, new()
    {
    }
}