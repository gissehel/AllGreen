using AllGreen.Lib.Core;
using AllGreen.Lib.Core.DomainModel.Script;
using AllGreen.Lib.Core.Fixture;
using System;

namespace AllGreen.Lib.DomainModel.Script
{
    public class FixturePropertyInfo<C> : IFixturePropertyInfo where C : class, IContext, new()
    {
        public TestUsingTableFixtureScript<C> Parent { get; private set; }

        public FixturePropertyInfo(TestUsingTableFixtureScript<C> parent)
        {
            Parent = parent;
        }

        public FixtureInfo FixtureInfo => Parent.FixtureInfo;

        public string CodeName { get; set; }

        public string Name => CodeName.Replace("_", " ");

        public Type Type { get; set; }

        public string PipedName => GetPipedName(PipedNameOptions.Default);

        public string GetPipedName(PipedNameOptions options) => Name;

        public Func<IFixture<C>, object> Getter { get; set; }

        public Action<IFixture<C>, object> Setter { get; set; }

        public Func<object, object> GetterForObject { get; set; }
    }
}