namespace UnitTests
{
    public class TestDecoratorService : ITestService
    {
        public TestDecoratorService(ITestService testService)
        {
            TestService = testService;
        }

        public ITestService TestService { get; }
    }
}
