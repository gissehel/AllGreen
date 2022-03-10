using AllGreen.Lib.DomainModel.Script;

namespace AllGreen.Lib.Core.Engine.Service
{
    public interface IScriptFileService
    {
        void GenerateScriptFile<C>(TestScript<C> testScript, string filename) where C : class, IContext<C>, new();
    }
}