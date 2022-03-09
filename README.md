# RandomSkunk.DependencyInjection.Decorator

A simple implementation of the decorator pattern for `Microsoft.Extensions.DependencyInjection`.

## Usage

To register a decorated service, call one of the `AddDecorated` extension methods. These methods
return a `IDecoratingBuilder<T>` object. Then add one or more decorators to the service by calling
`AddDecorator` on the builder.

For example, assume that you have the following interface, primary implementation, and decorator
implementation:

```c#
public interface IExampleService
{
    string GetSomething(int someValue);
}

public class ExampleService : IExampleService
{
    public string GetSomething(int someValue)
    {
        return $"Result from ExampleService.GetSomething({someValue})";
    }
}

public class LoggingExampleService : IExampleService
{
    private readonly IExampleService _exampleService;
    private readonly ILogger<LoggingExampleService> _logger;

    public LoggingExampleService(IExampleService exampleService,
        ILogger<LoggingExampleService> logger)
    {
        _exampleService = exampleService;
        _logger = logger;
    }

    public string GetSomething(int someValue)
    {
        var something = _exampleService.GetSomething(someValue);
        _logger.LogInformation(
            "Called IExampleService.GetSomething({SomeValue}), returning '{Something}'.",
            someValue, something);
        return something;
    }
}
```

Registering `IExampleService` as the service type and `ExampleService` as the primary
implementation type and `LoggingExampleService` as the decorator implementation type around it
becomes simply:

```c#
services.AddDecoratedSingleton<IExampleService, ExampleService>()
    .AddDecorator<IExampleService, LoggingExampleService>();
```
