using System;

namespace AllGreen.Lib.DomainModel
{
    public class FixtureInfo : ClassInfo
    {
        public FixtureInfo(Type t) : base(t)
        {
        }

        protected override string SuffixToRemove => "Fixture";
    }
}