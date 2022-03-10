using AllGreen.Lib.Core.Fixture;

namespace AllGreen.Lib.Core.Engine.Test
{
    public interface ITestMain<C> : IFluent where C : class, IContext, new()
    {
        /// <summary>
        /// Set the test as a runnable test. Otherwise, it will only be available for inclusion
        /// </summary>
        /// <returns></returns>
        ITestMain<C> IsRunnable();

        /// <summary>
        /// Use an ItemFixture (DoAction, DoCheck, DoAccept, DoReject)
        /// </summary>
        /// <typeparam name="F">The fixture to use</typeparam>
        /// <returns>The test using the ItemFixture</returns>
        ITestUsingItemFixture<C, F> Using<F>() where F : class, IFixture<C>, new();

        /// <summary>
        /// Use a Setup Fixture
        /// </summary>
        /// <typeparam name="F">The fixture</typeparam>
        /// <returns>The test using the Setup fixture</returns>
        ITestUsingUndecoratedSetupFixture<C, F> UsingSetup<F>() where F : class, IFixture<C>, new();

        /// <summary>
        /// Use a List fixture
        /// </summary>
        /// <typeparam name="F">The fixture</typeparam>
        /// <returns>The test using the List fixture</returns>
        ITestUsingUndecoratedOutputTableFixture<C, F> UsingList<F>() where F : class, IFixture<C>, new();

        /// <summary>
        /// Add a comment to the test
        /// </summary>
        /// <param name="comment">The comment to add</param>
        /// <returns>The rest of the test</returns>
        ITestMain<C> Comment(string comment);

        /// <summary>
        /// Include a sub test
        /// </summary>
        /// <typeparam name="T">The test class to include</typeparam>
        /// <returns>The rest of the test</returns>
        ITestMain<C> Include<T>() where T : class, ITest<C>, new();

        /// <summary>
        /// Stop the test. Should be called at the end of the test or the test will be empty.
        /// </summary>
        void EndTest();
    }
}