using AllGreen.Lib.DomainModel;

namespace AllGreen.Lib.Core.DomainModel
{
    public interface IPipedNamable
    {
        string PipedName { get; }

        string GetPipedName(PipedNameOptions options);
    }
}