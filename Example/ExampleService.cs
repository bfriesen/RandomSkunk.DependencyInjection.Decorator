namespace Example
{
    public class ExampleService : IExampleService
    {
        public string GetSomething(int someValue)
        {
            return $"Result from ExampleService.GetSomething('{someValue}')";
        }
    }
}
