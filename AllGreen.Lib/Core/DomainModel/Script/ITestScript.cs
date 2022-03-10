namespace AllGreen.Lib.Core.DomainModel.Script
{
    public interface ITestScript<C> : INamedTestScript where C : class, IContext, new()
    {
    }
}