using Microsoft.Extensions.Logging;

namespace Example
{
    public class LoggingExampleService : IExampleService
    {
        private readonly IExampleService _exampleService;
        private readonly ILogger<LoggingExampleService> _logger;

        public LoggingExampleService(IExampleService exampleService, ILogger<LoggingExampleService> logger)
        {
            _exampleService = exampleService;
            _logger = logger;
        }

        public string GetSomething(int someValue)
        {
            var something = _exampleService.GetSomething(someValue);
            _logger.LogInformation("Called IExampleService.GetSomething({SomeValue}), returning '{Something}'.", someValue, something);
            return something;
        }
    }
}
