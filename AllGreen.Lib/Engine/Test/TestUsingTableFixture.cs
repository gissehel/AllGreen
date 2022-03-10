using AllGreen.Lib.Core;
using AllGreen.Lib.Core.Engine.Test;
using AllGreen.Lib.Core.Fixture;
using AllGreen.Lib.DomainModel;
using AllGreen.Lib.DomainModel.Enumeration;
using AllGreen.Lib.DomainModel.Script;
using AllGreen.Lib.Utils.Reflexion;
using System;
using System.Linq.Expressions;

namespace AllGreen.Lib.Engine.Test
{
    public class TestUsingTableFixture<C, F> :
        ITestUsingUndecoratedSetupFixture<C, F>,
        ITestUsingDecoratedSetupFixture<C, F>,
        ITestUsingUndecoratedOutputTableFixture<C, F>,
        ITestUsingDecoratedOutputTableFixture<C, F>
        where C : class, IContext, new()
        where F : class, IFixture<C>, new()
    {
        private ITestMain<C> Parent { get; set; }

        private TestUsingTableFixtureScript<C> TestUsingTableFixtureScript { get; set; }

        public TestUsingTableFixture(ITestMain<C> parent, TestScript<C> testScript, FixtureTableKind kind)
        {
            Parent = parent;
            var fixtureInfo = new FixtureInfo(typeof(F));
            TestUsingTableFixtureScript = new TestUsingTableFixtureScript<C>(testScript, fixtureInfo, kind);
        }

        public ITestMain<C> EndUsing()
        {
            return Parent;
        }

        public TestUsingTableFixture<C, F> With<R>(params Expression<Func<R, string>>[] expressions)
        {
            foreach (var expression in expressions)
            {
                var name = expression.GetPropertyName();
                var getterForObject = ReflexionExtension.GetProperyGetter<R>(name);
                var fixturePropertyInfo = new FixturePropertyInfo<C>(TestUsingTableFixtureScript)
                {
                    CodeName = name,
                    Getter = null,
                    Setter = null,
                    GetterForObject = (o => getterForObject((R)o)),
                    Type = typeof(string),
                };
                TestUsingTableFixtureScript.FixturePropertyInfos.Add(fixturePropertyInfo);
            }
            return this;
        }

        public TestUsingTableFixture<C, F> With(params Expression<Func<F, object>>[] expressions)
        {
            foreach (var expression in expressions)
            {
                var name = expression.GetPropertyName();
                var getter = ReflexionExtension.GetProperyGetter<F>(name);
                var setter = ReflexionExtension.GetProperySetter<F>(name);
                // var getterForObject = new Func<object, object>(o => o.GetPropertyValue(name));
                Func<object, object> getterForObject = null;
                var fixturePropertyInfo = new FixturePropertyInfo<C>(TestUsingTableFixtureScript)
                {
                    CodeName = name,
                    Getter = new Func<IFixture<C>, object>(f => getter(f as F)),
                    Setter = new Action<IFixture<C>, object>((f, value) => setter(f as F, value)),
                    GetterForObject = getterForObject,
                    Type = ReflexionExtension.GetProperyType<F>(name),
                };
                TestUsingTableFixtureScript.FixturePropertyInfos.Add(fixturePropertyInfo);
            }
            return this;
        }

        public TestUsingTableFixture<C, F> With(params string[] names)
        {
            foreach (var name in names)
            {
                Func<IFixture<C>, object> getter = null;
                Action<IFixture<C>, object> setter = null;
                var getterForObject = new Func<object, object>(o => o.GetPropertyValue(name));
                var fixturePropertyInfo = new FixturePropertyInfo<C>(TestUsingTableFixtureScript)
                {
                    CodeName = name,
                    Getter = getter,
                    Setter = setter,
                    GetterForObject = getterForObject,
                    Type = typeof(string),
                };
                TestUsingTableFixtureScript.FixturePropertyInfos.Add(fixturePropertyInfo);
            }
            return this;
        }

        public TestUsingTableFixture<C, F> InsertValues(params object[] values)
        {
            var line = new TableFixtureLine<C>(TestUsingTableFixtureScript);

            foreach (var value in values)
            {
                line.PropertyValues.Add(value);
            }

            TestUsingTableFixtureScript.Lines.Add(line);
            return this;
        }

        public ITestUsingDecoratedSetupFixture<C, F> Enter(params object[] values)
        {
            return InsertValues(values);
        }

        public ITestUsingDecoratedOutputTableFixture<C, F> Check(params object[] values)
        {
            return InsertValues(values);
        }

        ITestUsingDecoratedSetupFixture<C, F> ITestUsingUndecoratedSetupFixture<C, F>.With(params Expression<Func<F, object>>[] expressions)
        {
            return With(expressions);
        }

        ITestUsingDecoratedOutputTableFixture<C, F> ITestUsingUndecoratedOutputTableFixture<C, F>.With(params string[] names)
        {
            return With(names);
        }

        ITestUsingDecoratedOutputTableFixture<C, F> ITestUsingUndecoratedOutputTableFixture<C, F>.With<R>(params Expression<Func<R, string>>[] expressions)
        {
            return With(expressions);
        }
    }
}