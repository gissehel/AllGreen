using AllGreen.Lib.Core;
using AllGreen.Lib.DomainModel;
using AllGreen.Lib.DomainModel.Enumeration;
using AllGreen.Lib.DomainModel.Script;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AllGreen.Lib.Engine.Service.CodeGeneration
{
    public class CodeGenerator<C> : ICodeGenerator where C : class, IContext<C>, new()
    {
        private StreamWriter Writer { get; set; }

        private string NamespaceValue { get; set; }

        private ICodeGeneratorContext GetContext(int indentLevel)
        {
            return new CodeGeneratorContext(this, indentLevel);
        }

        public CodeGenerator(StreamWriter writer, string namespaceValue)
        {
            Writer = writer;
            NamespaceValue = namespaceValue;
        }

        public void GenerateCode(TestScript<C> testScript)
        {
            GenerateCode(new TestScript<C>[] { testScript });
        }

        public void GenerateCode(IEnumerable<TestScript<C>> testScripts)
        {
            using (var mainContext = GetContext(-1))
            {
                GenerateUsing(mainContext, testScripts);

                using (var namespaceContext = GenerateNamespaceContext(mainContext))
                {
                    foreach (var testScript in testScripts)
                    {
                        using (var classContext = GenerateTestClass(namespaceContext, testScript.ClassName, typeof(C).Name))
                        {
                            using (var testContext = GenerateDoTest(classContext))
                            {
                                GenerateTest(testContext, testScript);
                            }
                        }
                    }
                }
            }
        }

        private List<string> FindNamespaces(IEnumerable<TestScript<C>> testScripts)
        {
            var namespacesSet = new HashSet<string>
            {
                typeof(C).Namespace,
                typeof(TestBase<C>).Namespace
            };

            foreach (var testScript in testScripts)
            {
                foreach (var testScriptItem in testScript.TestScriptItems)
                {
                    if (testScriptItem is TestIncludeScript<C> testIncludeScript)
                    {
                        namespacesSet.Add(testIncludeScript.TestInfo.Namespace);
                    }
                    else if (testScriptItem is TestUsingItemFixtureScript<C> testUsingItemFixtureScript)
                    {
                        namespacesSet.Add(testUsingItemFixtureScript.FixtureInfo.Namespace);
                    }
                    else if (testScriptItem is TestUsingTableFixtureScript<C> testUsingTableFixtureScript)
                    {
                        namespacesSet.Add(testUsingTableFixtureScript.FixtureInfo.Namespace);
                    }
                }
            }

            return namespacesSet.Where(s => s != NamespaceValue).OrderBy(s => s).ToList();
        }

        // FIXME : Make/use a better/robust implementation
        private string GetCSharpCodeString(string value) => string.Format("\"{0}\"", value.Replace("\n", "\\n"));

        private string GetCSharpCodeString(object value, Type type)
        {
            if (value == null)
            {
                return null;
            }
            if (type == typeof(int))
            {
                return ((int)value).ToString();
            }
            if (type == typeof(bool))
            {
                return ((bool)value).ToString();
            }
            return GetCSharpCodeString(value.ToString());
        }

        private void GenerateTest(ICodeGeneratorContext testContext, TestScript<C> testScript)
        {
            if (testScript.IsRunnable)
            {
                testContext.Add(".IsRunnable()");
                testContext.Add("");
            }
            foreach (var testScriptItem in testScript.TestScriptItems)
            {
                if (testScriptItem is TestIncludeScript<C> testIncludeScript)
                {
                    testContext.Add(string.Format(".Include<{0}>()", testIncludeScript.ClassName));
                }
                else if (testScriptItem is TestCommentScript<C> testCommentScript)
                {
                    testContext.Add(string.Format(".Comment({0})", GetCSharpCodeString(testCommentScript.Comment)));
                }
                else if (testScriptItem is TestUsingItemFixtureScript<C> testUsingItemFixtureScript)
                {
                    using (var newContext = testContext.GetSubContext())
                    {
                        GenerateTestUsingItemFixtureScript(newContext, testUsingItemFixtureScript);
                    }
                }
                else if (testScriptItem is TestUsingTableFixtureScript<C> testUsingTableFixtureScript)
                {
                    using (var newContext = testContext.GetSubContext())
                    {
                        GenerateTestUsingTableFixtureScript(newContext, testUsingTableFixtureScript);
                    }
                }
                else
                {
                    foreach (var line in testScriptItem.GetPipedName(new PipedNameOptions { Compact = true }).Split('\n'))
                    {
                        if (!string.IsNullOrEmpty(line))
                        {
                            testContext.Add(string.Format(".NotImplemented(\"{0}\")", line));
                        }
                    }
                }
                testContext.Add("");
            }
        }

        private void GenerateTestUsingTableFixtureScript(ICodeGeneratorContext testContext, TestUsingTableFixtureScript<C> testUsingTableFixtureScript)
        {
            testContext.AddBegin(string.Format(".Using{1}<{0}>()", testUsingTableFixtureScript.FixtureInfo.ClassName, testUsingTableFixtureScript.Kind.ToString()));
            testContext.AddEnd(".EndUsing()");

            string verb;
            if (testUsingTableFixtureScript.Kind == FixtureTableKind.Setup)
            {
                var columns = testUsingTableFixtureScript.FixturePropertyInfos.Select(f => string.Format("f => f.{0}", f.CodeName));
                testContext.Add(string.Format(".With({0})", string.Join(", ", columns.ToArray())));
                verb = "Enter";
            }
            else
            {
                var columns = testUsingTableFixtureScript.FixturePropertyInfos.Select(f => string.Format("r => r.{0}", f.CodeName));
                testContext.Add(string.Format(".With<{1}>({0})", string.Join(", ", columns.ToArray()), string.Format("{0}.Result", testUsingTableFixtureScript.FixtureInfo.ClassName)));
                verb = "Check";
            }

            foreach (var Line in testUsingTableFixtureScript.Lines)
            {
                GenerateTableFixtureLine(testContext, verb, Line);
            }
        }

        private void GenerateTableFixtureLine(ICodeGeneratorContext testContext, string verb, TableFixtureLine<C> line)
        {
            testContext.Add(string.Format(".{0}({1})", verb, string.Join(", ", line.PropertyValues.Select(p => GetCSharpCodeString(p, p.GetType())).ToArray())));
        }

        private void GenerateTableFixtureLine(ICodeGeneratorContext testContext, TableFixtureLine<C> line)
        {
            testContext.Add(string.Format(".NotImplemented({0})", GetCSharpCodeString(line.PipedName)));
        }

        private void GenerateTestUsingItemFixtureScript(ICodeGeneratorContext testContext, TestUsingItemFixtureScript<C> testUsingItemFixtureScript)
        {
            testContext.AddBegin(string.Format(".Using<{0}>()", testUsingItemFixtureScript.FixtureInfo.ClassName));
            testContext.AddEnd(".EndUsing()");
            foreach (var fixtureItem in testUsingItemFixtureScript.FixtureItems)
            {
                if (fixtureItem.Kind == FixtureItemKind.Comment)
                {
                    testContext.Add(string.Format(".Comment({0})", GetCSharpCodeString(fixtureItem.Comment)));
                }
                else
                {
                    var methodName = fixtureItem.CodeName;
                    var parametersBuilder = new StringBuilder();
                    var preParenthesisBuilder = new StringBuilder();
                    var postParenthesisBuilder = new StringBuilder();
                    var argumentsBuilder = new StringBuilder();
                    var resultBuilder = new StringBuilder();

                    if (fixtureItem.Args.Count > 0)
                    {
                        preParenthesisBuilder.Append("(");
                        for (int argIndex = 0; argIndex < fixtureItem.Args.Count; argIndex++)
                        {
                            if (argIndex != 0)
                            {
                                parametersBuilder.Append(", ");
                            }
                            parametersBuilder.Append(GetCSharpCodeString(fixtureItem.Args[argIndex], fixtureItem.ArgTypes[argIndex]));
                        }
                        postParenthesisBuilder.Append(")");
                    }
                    if (fixtureItem.Kind == FixtureItemKind.Check)
                    {
                        resultBuilder.Append(", ");
                        resultBuilder.Append(GetCSharpCodeString(fixtureItem.ExpectedCheckResult));
                    }
                    testContext.Add
                    (
                        string.Format
                        (
                            ".Do{0}(f => f.{1}({2}){3})",
                            fixtureItem.Kind.ToString(),
                            methodName,
                            parametersBuilder.ToString(),
                            resultBuilder.ToString()
                        )
                    );
                }
            }
        }

        private void GenerateUsing(ICodeGeneratorContext mainContext, IEnumerable<TestScript<C>> testScripts)
        {
            foreach (var usingName in FindNamespaces(testScripts))
            {
                mainContext.Add(string.Format("using {0};", usingName));
            }
            mainContext.Add("");
        }

        public ICodeGeneratorContext GenerateNamespaceContext(ICodeGeneratorContext context)
        {
            var subContext = context.GetSubContext();
            subContext.AddBegin(string.Format("namespace {0}", NamespaceValue));
            subContext.AddBegin("{");
            subContext.AddEnd("}");
            return subContext;
        }

        public ICodeGeneratorContext GenerateTestClass(ICodeGeneratorContext context, string className, string contextName)
        {
            var subContext = context.GetSubContext();
            subContext.AddBegin(string.Format("public class {0} : TestBase<{1}>", className, contextName));
            subContext.AddBegin("{");
            subContext.AddEnd("}");
            return subContext;
        }

        public ICodeGeneratorContext GenerateDoTest(ICodeGeneratorContext context)
        {
            var subContext = context.GetSubContext();
            subContext.AddBegin("public override void DoTest() => StartTest()");
            subContext.AddBegin("");
            subContext.AddEnd(1, ".EndTest();");
            return subContext;
        }

        public void WriteLine(int indentLevel, string line)
        {
            if (string.IsNullOrEmpty(line))
            {
                Writer.WriteLine(string.Empty);
            }
            else
            {
                for (int level = 0; level < indentLevel; level++)
                {
                    Writer.Write("    ");
                }
                Writer.WriteLine(line.Trim(' ', '\t', '\r', '\n'));
            }
        }
    }
}