using AllGreen.Lib.Core;
using AllGreen.Lib.Core.Engine.Test;
using AllGreen.Lib.Core.Fixture;
using AllGreen.Lib.DomainModel;
using AllGreen.Lib.DomainModel.Enumeration;
using AllGreen.Lib.DomainModel.Script;
using AllGreen.Lib.Utils.Reflexion;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace AllGreen.Lib.Engine.Test
{
    public class TestUsingItemFixture<C, F> : ITestUsingItemFixture<C, F> where C : class, IContext, new() where F : class, IFixture<C>, new()
    {
        private ITestMain<C> Parent { get; set; }

        private TestUsingItemFixtureScript<C> TestUsingItemFixtureScript { get; set; }

        public TestUsingItemFixture(ITestMain<C> parent, TestScript<C> testScript)
        {
            Parent = parent;
            var fixtureInfo = new FixtureInfo(typeof(F));
            TestUsingItemFixtureScript = new TestUsingItemFixtureScript<C>(testScript, fixtureInfo);
        }

        public ITestMain<C> EndUsing()
        {
            return Parent;
        }

        public ITestUsingItemFixture<C, F> Comment(string comment)
        {
            var fixtureItem = new FixtureItem<C>(FixtureItemKind.Comment, comment);

            TestUsingItemFixtureScript.FixtureItems.Add(fixtureItem);

            return this;
        }

        private static Action<F> GenerateAction(string name, System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<object, Type>> arguments)
        {
            var expressionArguments = arguments.Select(a => Expression.Constant(a.Key)).ToArray();
            var fixture = Expression.Parameter(typeof(F), "f");
            var body = Expression.Call(fixture, name, null, expressionArguments);
            var lambdaExpr = Expression.Lambda<Action<F>>(body, fixture);
            var code = lambdaExpr.Compile();
            return code;
        }

        private static Func<F, X> GenerateFunc<X>(string name, System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<object, Type>> arguments)
        {
            var expressionArguments = arguments.Select(a => Expression.Constant(a.Key)).ToArray();
            var fixture = Expression.Parameter(typeof(F), "f");
            var body = Expression.Call(fixture, name, null, expressionArguments);
            var lambdaExpr = Expression.Lambda<Func<F, X>>(body, fixture);
            var code = lambdaExpr.Compile();
            return code;
        }

        public ITestUsingItemFixture<C, F> DoAction(Expression<Action<F>> expression)
        {
            var name = expression.GetMethodName();
            var arguments = expression.GetMethodArguments().ToList();

            Action<F> code = GenerateAction(name, arguments);

            var fixtureItem = new FixtureItem<C>(FixtureItemKind.Action, name)
            {
                ActionCode = (f, args) => code(f as F),
            };

            foreach (var argument in arguments)
            {
                fixtureItem.Args.Add(argument.Key);
                fixtureItem.ArgTypes.Add(argument.Value);
            }

            TestUsingItemFixtureScript.FixtureItems.Add(fixtureItem);

            return this;
        }

        public ITestUsingItemFixture<C, F> DoCheck(Expression<Func<F, string>> expression, string result)
        {
            var name = expression.GetMethodName();
            var arguments = expression.GetMethodArguments().ToList();

            Func<F, string> code = GenerateFunc<string>(name, arguments);

            var fixtureItem = new FixtureItem<C>(FixtureItemKind.Check, name)
            {
                CheckCode = (f, args) => code(f as F),
                ExpectedCheckResult = result,
            };

            foreach (var argument in arguments)
            {
                fixtureItem.Args.Add(argument.Key);
                fixtureItem.ArgTypes.Add(argument.Value);
            }

            TestUsingItemFixtureScript.FixtureItems.Add(fixtureItem);

            return this;
        }

        private ITestUsingItemFixture<C, F> DoVerify(Expression<Func<F, bool>> expression, FixtureItemKind kind, bool expectedResult)
        {
            var name = expression.GetMethodName();
            var arguments = expression.GetMethodArguments().ToList();

            Func<F, bool> code = GenerateFunc<bool>(name, arguments);

            var fixtureItem = new FixtureItem<C>(kind, expression.GetMethodName())
            {
                AcceptRejectCode = (f, args) => code(f as F),
                ExpectedAcceptRejectResult = expectedResult,
            };

            foreach (var argument in arguments)
            {
                fixtureItem.Args.Add(argument.Key);
            }

            TestUsingItemFixtureScript.FixtureItems.Add(fixtureItem);

            return this;
        }

        public ITestUsingItemFixture<C, F> DoAccept(Expression<Func<F, bool>> expression) => DoVerify(expression, FixtureItemKind.Accept, true);

        public ITestUsingItemFixture<C, F> DoReject(Expression<Func<F, bool>> expression) => DoVerify(expression, FixtureItemKind.Reject, false);
    }
}