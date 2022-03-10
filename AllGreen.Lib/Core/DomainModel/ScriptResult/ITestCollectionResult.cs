using System;

namespace AllGreen.Lib.Core.DomainModel.ScriptResult
{
    public interface ITestCollectionResult
    {
        /// <summary>
        /// The total number of items contains in this test part (code, check or event comment)
        /// </summary>
        int TestItemCount { get; }

        /// <summary>
        /// The total number of items contains in this test part that end up executing code (so comments aren't counted)
        /// </summary>
        int TestItemCodeCount { get; }

        /// <summary>
        /// The total number of items contains in this test part that actually return a success or an error (so neither actions or comments are counted here)
        /// </summary>
        int TestItemResultCount { get; }

        /// <summary>
        /// Total number of items contains in this test part that have an Unknown status
        /// </summary>
        int TestItemUnknownCount { get; }

        /// <summary>
        /// Total number of items contains in this test part that have a Success status
        /// </summary>
        int TestItemSuccessCount { get; }

        /// <summary>
        /// Total number of items contains in this test part that have an Error status
        /// </summary>
        int TestItemErrorCount { get; }

        /// <summary>
        /// Total number of items contains in this test part that have a Failure status
        /// </summary>
        int TestItemFailureCount { get; }

        /// <summary>
        /// Total number of items contains in this test part that have a Comment status
        /// </summary>
        int TestItemCommentCount { get; }

        /// <summary>
        /// True if that part of the test is a success.
        /// </summary>
        bool Success { get; }

        /// <summary>
        /// The start time of execution of the first item
        /// </summary>
        DateTime? Start { get; }

        /// <summary>
        /// The stop time of execution of the last item
        /// </summary>
        DateTime? Stop { get; }

        /// <summary>
        /// The total duration of the test part
        /// </summary>
        TimeSpan? Duration { get; }
    }
}