using Example;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var services = new ServiceCollection();

services.AddLogging((ILoggingBuilder builder) =>
{
    builder.AddConsole();
});

services.AddDecoratedSingleton<IExampleService, ExampleService>()
    .AddDecorator<IExampleService, LoggingExampleService>();

using var serviceProvider = services.BuildServiceProvider();

var exampleService = serviceProvider.GetRequiredService<IExampleService>();

Console.WriteLine(exampleService.GetSomething(123));
