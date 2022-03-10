using AllGreen.Lib.Core;
using AllGreen.Lib.Core.Engine.Service;
using AllGreen.Lib.Core.Fixture;
using AllGreen.Lib.DomainModel.Enumeration;
using AllGreen.Lib.DomainModel.Script;
using AllGreen.Lib.DomainModel.ScriptResult;
using System;
using System.Linq;

namespace AllGreen.Lib.Engine.Service
{
    public class TestRunnerService : ITestRunnerService
    {
        public TestRunnerService()
        {
        }

        public TestScriptResult<C> RunTest<C>(TestScript<C> testScript) where C : class, IContext<C>, new()
        {
            if (!testScript.IsRunnable)
            {
                return null;
            }
            var testScriptResult = new TestScriptResult<C>(testScript);

            var context = new C
            {
                TestScript = testScript,
                TestScriptResult = testScriptResult
            };

            testScriptResult.Context = context;

            context.OnTestStart();

            RunTest(testScript, testScriptResult, context);

            context.OnTestStop();

            return testScriptResult;
        }

        public TestScriptResult<C> RunTest<C>(TestScript<C> testScript, C context) where C : class, IContext<C>, new()
        {
            if (!testScript.IsRunnable)
            {
                return null;
            }
            var testScriptResult = new TestScriptResult<C>(testScript);

            context.TestScript = testScript;
            context.TestScriptResult = testScriptResult;

            testScriptResult.Context = context;

            context.OnTestStart();

            RunTest(testScript, testScriptResult, context);

            context.OnTestStop();

            return testScriptResult;
        }

        public void RunTest<C>(TestScript<C> testScript, TestScriptResult<C> testScriptResult, C context) where C : class, IContext, new()
        {
            foreach (var testScriptItem in testScript.TestScriptItems)
            {
                switch (testScriptItem)
                {
                    case TestUsingItemFixtureScript<C> testUsingItemFixtureScript:
                        RunTestUsingItemFixture(testUsingItemFixtureScript, testScriptResult, context);
                        break;

                    case TestUsingTableFixtureScript<C> testUsingTableFixtureScript:
                        RunTestUsingTableFixture(testUsingTableFixtureScript, testScriptResult, context);
                        break;

                    case TestCommentScript<C> testCommentScript:
                        var testCommentScriptResult = new TestCommentScriptResult<C>(testScriptResult, testCommentScript);
                        break;

                    case TestIncludeScript<C> testIncludeScript:
                        RunTestInclude(testIncludeScript, testScriptResult, context);
                        break;

                    default:
                        break;
                }
            }
        }

        private void RunTestInclude<C>(TestIncludeScript<C> testIncludeScript, TestScriptResult<C> testScriptResult, C context) where C : class, IContext, new()
        {
            var testSubScriptResult = new TestScriptResult<C>(testIncludeScript.TestScript);
            var testIncludeScriptResult = new TestIncludeScriptResult<C>(testScriptResult, testIncludeScript, testSubScriptResult);

            RunTest(testIncludeScript.TestScript, testSubScriptResult, context);
        }

        private static void RunTestUsingTableFixture<C>(TestUsingTableFixtureScript<C> testUsingTableFixtureScript, TestScriptResult<C> testScriptResult, C context) where C : class, IContext, new()
        {
            var fixtureInfo = testUsingTableFixtureScript.FixtureInfo;
            var fixture = Activator.CreateInstance(fixtureInfo.Type) as IFixture<C>;
            fixture.Context = context;

            var testUsingTableFixtureScriptResult = new TestUsingTableFixtureScriptResult<C>(testScriptResult, testUsingTableFixtureScript);

            if (testUsingTableFixtureScript.Kind == FixtureTableKind.Setup)
            {
                RunTestOnSetup(testUsingTableFixtureScript, testUsingTableFixtureScriptResult, fixture);
            }
            else
            {
                RunTestOnOutputTable(testUsingTableFixtureScript, testUsingTableFixtureScriptResult, fixture);
            }
        }

        private static void RunTestOnOutputTable<C>(TestUsingTableFixtureScript<C> testUsingTableFixtureScript, TestUsingTableFixtureScriptResult<C> testUsingTableFixtureScriptResult, IFixture<C> fixture) where C : class, IContext, new()
        {
            try
            {
                var results = fixture.OnQuery().ToList();

                testUsingTableFixtureScriptResult.TestUsingOutputTableQueryScriptResult.Stop = DateTime.Now;

                int lineNumber = 0;

                foreach (var line in testUsingTableFixtureScript.Lines)
                {
                    var tableFixtureLineResult = new TableFixtureLineResult<C>(testUsingTableFixtureScriptResult, line) { Start = DateTime.Now };

                    if (lineNumber < results.Count)
                    {
                        try
                        {
                            var lineResult = results[lineNumber];
                            int position = 0;
                            var state = tableFixtureLineResult.State;
                            foreach (var fixturePropertyInfo in testUsingTableFixtureScript.FixturePropertyInfos)
                            {
                                var expectedValue = line.PropertyValues[position].ToString();
                                var actualValue = fixturePropertyInfo.GetterForObject(lineResult).ToString();

                                tableFixtureLineResult.ActualPropertyValues.Add(actualValue);

                                if (expectedValue != actualValue)
                                {
                                    state = TestItemState.Error;
                                }

                                position++;
                            }
                            if (state == TestItemState.Unknown)
                            {
                                state = TestItemState.Success;
                            }
                            tableFixtureLineResult.State = state;
                        }
                        catch (Exception ex)
                        {
                            tableFixtureLineResult.State = TestItemState.Failure;
                            tableFixtureLineResult.Exception = ex;
                            tableFixtureLineResult.FailureShortMessage = ex.Message;
                        }
                    }
                    else
                    {
                        tableFixtureLineResult.State = TestItemState.Error;
                        tableFixtureLineResult.IsMissing = true;
                    }
                    tableFixtureLineResult.Stop = DateTime.Now;
                    lineNumber++;
                }
                for (; lineNumber < results.Count; lineNumber++)
                {
                    var tableFixtureLineResult = new TableFixtureLineResult<C>(testUsingTableFixtureScriptResult, null)
                    {
                        IsSurplus = true,
                        State = TestItemState.Error,
                        Start = DateTime.Now,
                    };

                    try
                    {
                        var lineResult = results[lineNumber];
                        foreach (var fixturePropertyInfo in testUsingTableFixtureScript.FixturePropertyInfos)
                        {
                            tableFixtureLineResult.ActualPropertyValues.Add(fixturePropertyInfo.GetterForObject(lineResult));
                        }
                    }
                    catch (Exception ex)
                    {
                        tableFixtureLineResult.State = TestItemState.Failure;
                        tableFixtureLineResult.Exception = ex;
                        tableFixtureLineResult.FailureShortMessage = ex.Message;
                    }
                    tableFixtureLineResult.Stop = DateTime.Now;
                }
            }
            catch
            {
                foreach (var line in testUsingTableFixtureScript.Lines)
                {
                    var tableFixtureLineResult = new TableFixtureLineResult<C>(testUsingTableFixtureScriptResult, line)
                    {
                        State = TestItemState.Failure,
                        Start = DateTime.Now,
                    };
                    tableFixtureLineResult.Stop = DateTime.Now;
                }
            }
        }

        private static void RunTestOnSetup<C>(TestUsingTableFixtureScript<C> testUsingTableFixtureScript, TestUsingTableFixtureScriptResult<C> testUsingTableFixtureScriptResult, IFixture<C> fixture) where C : class, IContext, new()
        {
            foreach (var line in testUsingTableFixtureScript.Lines)
            {
                var tableFixtureLineResult = new TableFixtureLineResult<C>(testUsingTableFixtureScriptResult, line) { Start = DateTime.Now };

                try
                {
                    RunTestOnSetupLine(testUsingTableFixtureScript, tableFixtureLineResult, fixture, line);
                }
                catch (Exception ex)
                {
                    tableFixtureLineResult.State = TestItemState.Failure;
                    tableFixtureLineResult.Exception = ex;
                    tableFixtureLineResult.FailureShortMessage = ex.Message;
                }

                tableFixtureLineResult.Stop = DateTime.Now;
            }
        }

        private static void RunTestOnSetupLine<C>(TestUsingTableFixtureScript<C> testUsingTableFixtureScript, TableFixtureLineResult<C> tableFixtureLineResult, IFixture<C> fixture, TableFixtureLine<C> line) where C : class, IContext, new()
        {
            int position = 0;
            foreach (var fixturePropertyInfo in testUsingTableFixtureScript.FixturePropertyInfos)
            {
                var value = line.PropertyValues[position];
                fixturePropertyInfo.Setter(fixture, value);
                position++;
            }
            var result = fixture.OnEnterSetup();
            if (result)
            {
                tableFixtureLineResult.State = TestItemState.Success;
            }
            else
            {
                tableFixtureLineResult.State = TestItemState.Error;
            }
        }

        private static void RunTestUsingItemFixture<C>(TestUsingItemFixtureScript<C> testUsingItemFixtureScript, TestScriptResult<C> testScriptResult, C context) where C : class, IContext, new()
        {
            var fixtureInfo = testUsingItemFixtureScript.FixtureInfo;
            var fixture = Activator.CreateInstance(fixtureInfo.Type) as IFixture<C>;
            fixture.Context = context;

            var testUsingItemFixtureScriptResult = new TestUsingItemFixtureScriptResult<C>(testScriptResult, testUsingItemFixtureScript);

            foreach (var fixtureItem in testUsingItemFixtureScript.FixtureItems)
            {
                var fixtureItemResult = new FixtureItemResult<C>(testUsingItemFixtureScriptResult, fixtureItem) { Start = DateTime.Now };
                try
                {
                    switch (fixtureItem.Kind)
                    {
                        case FixtureItemKind.Action:
                            fixtureItem.ActionCode(fixture, fixtureItem.Args);
                            fixtureItemResult.State = TestItemState.Success;
                            break;

                        case FixtureItemKind.Check:
                            fixtureItemResult.ActualResult = fixtureItem.CheckCode(fixture, fixtureItem.Args);
                            if (fixtureItemResult.ActualResult != fixtureItem.ExpectedCheckResult)
                            {
                                fixtureItemResult.State = TestItemState.Error;
                            }
                            else
                            {
                                fixtureItemResult.State = TestItemState.Success;
                            }
                            break;

                        case FixtureItemKind.Accept:
                        case FixtureItemKind.Reject:
                            var actualResult = fixtureItem.AcceptRejectCode(fixture, fixtureItem.Args);
                            fixtureItemResult.ActualResult = actualResult ? "true" : "false";
                            if (actualResult != fixtureItem.ExpectedAcceptRejectResult)
                            {
                                fixtureItemResult.State = TestItemState.Error;
                            }
                            else
                            {
                                fixtureItemResult.State = TestItemState.Success;
                            }
                            break;

                        case FixtureItemKind.Comment:
                            fixtureItemResult.State = TestItemState.Comment;
                            break;

                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    fixtureItemResult.State = TestItemState.Failure;
                    fixtureItemResult.Exception = ex;
                    fixtureItemResult.FailureShortMessage = ex.Message;
                }
                fixtureItemResult.Stop = DateTime.Now;
            }
        }
    }
}