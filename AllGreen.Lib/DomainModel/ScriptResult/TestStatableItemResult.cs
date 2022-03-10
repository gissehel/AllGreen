using AllGreen.Lib.Core.DomainModel.ScriptResult;
using AllGreen.Lib.DomainModel.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AllGreen.Lib.DomainModel.ScriptResult
{
    public abstract class TestStatableItemResult : PipedNamableScriptResult, ITestStatableItemResult
    {
        public TestItemState State { get; set; } = TestItemState.Unknown;

        public sealed override string GetJsonResult(JsonOptions options) => GetJsonObject(
            new List<string>
            {
                GetJsonObjectItem("isSuccess", GetJsonScalar(Success)),
                GetJsonObjectItemOrNull("actual", GetJsonActualValue()),
                GetJsonObjectItem("state", GetJsonScalar(GetJsonValue(State))),
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

        public abstract string GetJsonActualValue();

        public string GetJsonValue(TestItemState value)
        {
            switch (value)
            {
                case TestItemState.Unknown: return "unknown";
                case TestItemState.Success: return "success";
                case TestItemState.Error: return "error";
                case TestItemState.Failure: return "failure";
                case TestItemState.Comment: return "comment";
                case TestItemState.None: return "none";
                default: return "none";
            }
        }

        public Exception Exception { get; set; }

        public string FailureShortMessage { get; set; }

        public int TestItemCount => 1;

        public int TestItemCodeCount => IsTestCode ? 1 : 0;

        public int TestItemResultCount => IsTestResult ? 1 : 0;

        public int TestItemUnknownCount => State == TestItemState.Unknown ? 1 : 0;

        public int TestItemSuccessCount => State == TestItemState.Success ? 1 : 0;

        public int TestItemErrorCount => State == TestItemState.Error ? 1 : 0;

        public int TestItemFailureCount => State == TestItemState.Failure ? 1 : 0;

        public int TestItemCommentCount => State == TestItemState.Comment ? 1 : 0;

        public bool Success => State == TestItemState.Success || State == TestItemState.Comment;

        public bool IsTestCode { get; set; }

        public bool IsTestResult { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? Stop { get; set; }

        public TimeSpan? Duration
        {
            get
            {
                if ((!Start.HasValue) || (!Stop.HasValue))
                {
                    return null;
                }
                return Stop.Value - Start.Value;
            }
        }
    }
}