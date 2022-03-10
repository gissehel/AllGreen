namespace AllGreen.Lib.Core.DomainModel.Script
{
    public interface ITestScriptItem : IPipedNamableScript
    {
    }

    public interface ITestScriptItem<C> : ITestScriptItem where C : class, IContext, new()
    {
    }
}