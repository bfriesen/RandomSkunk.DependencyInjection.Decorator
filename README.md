# RandomSkunk.DependencyInjection.Decorator

A simple implementation of the decorator pattern for [Microsoft.Extensions.DependencyInjection].

## Usage

To register a decorated service, add the service to the service collection by calling one of the
`AddDecorated`, `AddDecoratedTransient`, `AddDecoratedScoped`, or `AddDecoratedSingleton` extension
methods. Each of these methods returns a `IDecoratingBuilder<T>` object with an `AddDecorator`
method for adding decorators to the service. Multiple decorators can be added to the service by
calling `AddDecorator` on the builder multiple times.

```c#
services.AddDecoratedSingleton<IExampleService, ExampleService>()
    .AddDecorator<IExampleService, LoggingExampleService>();
```

```c#
services.AddDecoratedTransient<IExampleService>(() => new ExampleService())
    .AddDecorator((service, serviceProvider) =>
    {
        var logger = serviceProvider.GetRequiredService<ILogger<LoggingExampleService>>();
        return new LoggingExampleService(service, logger);
    });
```

These examples assume the following interface, primary implementation, and decorator
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
        return $"Result from ExampleService.GetSomething('{someValue}')";
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
            "Called IExampleService.GetSomething('{SomeValue}'), returning '{Something}'.",
            someValue, something);
        return something;
    }
}
```

[Microsoft.Extensions.DependencyInjection]: https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection
