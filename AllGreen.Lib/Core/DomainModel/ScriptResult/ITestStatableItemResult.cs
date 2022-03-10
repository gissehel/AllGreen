using AllGreen.Lib.DomainModel.Enumeration;

namespace AllGreen.Lib.Core.DomainModel.ScriptResult
{
    public interface ITestStatableItemResult : ITestCollectionResult, IPipedNamableScriptResult
    {
        TestItemState State { get; }

        bool IsTestCode { get; }

        bool IsTestResult { get; }
    }
}