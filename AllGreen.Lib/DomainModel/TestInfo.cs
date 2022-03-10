using System;

namespace AllGreen.Lib.DomainModel
{
    public class TestInfo : ClassInfo
    {
        public TestInfo(Type t) : base(t)
        {
        }

        protected override string SuffixToRemove => "Test";
    }
}