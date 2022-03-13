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

/* ...or... */

//services.AddDecoratedTransient<IExampleService>(() => new ExampleService())
//    .AddDecorator((service, serviceProvider) =>
//    {
//        var logger = serviceProvider.GetRequiredService<ILogger<LoggingExampleService>>();
//        return new LoggingExampleService(service, logger);
//    });

using var serviceProvider = services.BuildServiceProvider();

var exampleService = serviceProvider.GetRequiredService<IExampleService>();

Console.WriteLine(exampleService.GetSomething(123));
