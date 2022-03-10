using AllGreen.Lib.Core;
using AllGreen.Lib.Core.Engine.Service;
using AllGreen.Lib.DomainModel.Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AllGreen.Lib
{
    public class TestFinder<C> where C : class, IContext, new()
    {
        private Dictionary<string, TestScript<C>> _testScripts = null;
        private Dictionary<string, TestScript<C>> TestScripts => _testScripts ?? (_testScripts = GetTestScripts());

        public Assembly Assembly { get; set; }

        private Dictionary<string, TestScript<C>> GetTestScripts()
        {
            var testScripts = new Dictionary<string, TestScript<C>>();
            var allRunnableTestScripts = Assembly
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(TestBase<C>)))
                .Select(t => (Activator.CreateInstance(t) as TestBase<C>).GetTestScript())
                .Where(s => s != null)
                .Where(s => s.IsRunnable)
                ;

            foreach (var testScript in allRunnableTestScripts)
            {
                testScripts[testScript.Name] = testScript;
            }
            return testScripts;
        }

        public TestScript<C> GetTestScript(string name)
        {
            if (TestScripts.ContainsKey(name))
            {
                return TestScripts[name];
            }
            return null;
        }

        public IEnumerable<string> GetNames()
        {
            return TestScripts.Keys.AsEnumerable();
        }

    }
}
