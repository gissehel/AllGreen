using AllGreen.Lib.DomainModel.Script;

namespace AllGreen.Lib.Core.Engine.Test
{
    public interface ITestScriptSettable<C> where C : class, IContext, new()
    {
        TestScript<C> TestScript { set; }
    }
}