using AllGreen.Lib.Core.DomainModel.ScriptResult;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AllGreen.Lib.DomainModel.ScriptResult
{
    public abstract class TestScriptItemResult : PipedNamableScriptResult, ITestScriptItemResult
    {
        // public abstract string PipedName { get; }

        public abstract IEnumerable<ITestCollectionResult> TestCollectionResults { get; }

        public override string GetJsonResult(JsonOptions options) => GetJsonObject(
            new List<string>
            {
                GetJsonObjectItem("isSuccess", GetJsonScalar(Success)),
                GetJsonObjectItemOrNull("start", GetJsonScalarOrNull(Start)),
                GetJsonObjectItemOrNull("stop", GetJsonScalarOrNull(Stop)),
                GetJsonObjectItemOrNull("duration", GetJsonScalarOrNull(Duration)),
                GetJsonObjectItem("items", GetJsonScalar(TestItemCount)),
                GetJsonObjectItem("code", GetJsonScalar(TestItemCodeCount)),
                GetJsonObjectItem("result", GetJsonScalar(TestItemResultCount)),

                GetJsonObjectItem("unknown", GetJsonScalar(TestItemUnknownCount)),
                GetJsonObjectItem("success", GetJsonScalar(TestItemSuccessCount)),
                GetJsonObjectItem("error", GetJsonScalar(TestItemErrorCount)),
                GetJsonObjectItem("failure", GetJsonScalar(TestItemFailureCount)),
                GetJsonObjectItem("comment", GetJsonScalar(TestItemCommentCount)),
            }
        );

        public abstract IEnumerable<IPipedNamableScriptResult> SubElements { get; }

        public sealed override IEnumerable<string> GetJsonObjectItems(JsonOptions options) =>
            GetJsonObjectTestItems(options).Concat(
                new List<string> {
                    GetJsonObjectItem("results", GetJsonResult(options)),
                    GetJsonObjectItem("content", GetJsonArray(
                        SubElements == null ? null : SubElements.Select(subElement => subElement.GetJson(options))
                    )),
                }
            );

        public int TestItemCount => TestCollectionResults.Sum(c => c.TestItemCount);
        public int TestItemCodeCount => TestCollectionResults.Sum(c => c.TestItemCodeCount);
        public int TestItemResultCount => TestCollectionResults.Sum(c => c.TestItemResultCount);

        public int TestItemUnknownCount => TestCollectionResults.Sum(c => c.TestItemUnknownCount);
        public int TestItemSuccessCount => TestCollectionResults.Sum(c => c.TestItemSuccessCount);
        public int TestItemErrorCount => TestCollectionResults.Sum(c => c.TestItemErrorCount);
        public int TestItemFailureCount => TestCollectionResults.Sum(c => c.TestItemFailureCount);
        public int TestItemCommentCount => TestCollectionResults.Sum(c => c.TestItemCommentCount);

        public bool Success => TestCollectionResults.All(c => c.Success);

        public DateTime? Start => TestCollectionResults.Count() == 0 ? null : new DateTime?(TestCollectionResults.Select(c => c.Start).Where(time => time.HasValue).Min(time => time.Value));

        public DateTime? Stop => TestCollectionResults.Count() == 0 ? null : new DateTime?(TestCollectionResults.Select(c => c.Stop).Where(time => time.HasValue).Max(time => time.Value));

        public TimeSpan? Duration
        {
            get
            {
                var start = Start;
                var stop = Stop;

                if ((!start.HasValue) || (!stop.HasValue))
                {
                    return null;
                }
                return stop.Value - start.Value;
            }
        }
    }
}