using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using Xunit;

namespace UnitTests
{
    public class DecoratingBuilderExtensionsTests
    {
        [Fact(DisplayName = "AddDecorator extension method 1 adds decorator to builder")]
        public void AddDecoratorExtensionMethod1HappyPath()
        {
            var mainService = new Mock<ITestService>().Object;
            var decoratorService = new Mock<ITestService>().Object;

            var mockMainServiceFactory = new Mock<Func<IServiceProvider, ITestService>>();
            mockMainServiceFactory.Setup(m => m.Invoke(It.IsAny<IServiceProvider>()))
                .Returns(mainService);
            var mainServiceFactory = mockMainServiceFactory.Object;

            var mockDecoratorFactory = new Mock<Func<ITestService, ITestService>>();
            mockDecoratorFactory.Setup(m => m.Invoke(It.IsAny<ITestService>()))
                .Returns(decoratorService);
            var decoratorFactory = mockDecoratorFactory.Object;

            var mockServiceProvider = new Mock<IServiceProvider>();
            var serviceProvider = mockServiceProvider.Object;

            var builder = new DecoratingBuilder<ITestService>(mainServiceFactory);

            builder.AddDecorator(decoratorFactory);

            builder.ServiceFactory.Should().NotBeSameAs(mainServiceFactory);

            var actualTestService = builder.Build(serviceProvider);

            actualTestService.Should().BeSameAs(decoratorService);

            mockMainServiceFactory.Verify(m => m.Invoke(serviceProvider), Times.Once());
            mockDecoratorFactory.Verify(m => m.Invoke(mainService), Times.Once());

            mockMainServiceFactory.VerifyNoOtherCalls();
            mockDecoratorFactory.VerifyNoOtherCalls();
            mockServiceProvider.VerifyNoOtherCalls();
        }

        [Fact(DisplayName = "AddDecorator extension method 1 throws when decoratorFactory is null")]
        public void AddDecoratorExtensionMethod1SadPath()
        {
            var builder = new Mock<IDecoratingBuilder<ITestService>>().Object;
            Func<ITestService, ITestService>? decoratorFactory = null;

            Action act = () => builder.AddDecorator(decoratorFactory!);

            act.Should().Throw<ArgumentException>();
        }

        [Fact(DisplayName = "AddDecorator extension method 2 adds decorator to builder")]
        public void AddDecoratorExtensionMethod2HappyPath()
        {
            var mainService = new Mock<ITestService>().Object;

            var mockMainServiceFactory = new Mock<Func<IServiceProvider, ITestService>>();
            mockMainServiceFactory.Setup(m => m.Invoke(It.IsAny<IServiceProvider>()))
                .Returns(mainService);
            var mainServiceFactory = mockMainServiceFactory.Object;

            var mockServiceProvider = new Mock<IServiceProvider>();
            var serviceProvider = mockServiceProvider.Object;

            var builder = new DecoratingBuilder<ITestService>(mainServiceFactory);

            builder.AddDecorator<ITestService, TestDecoratorService>();

            builder.ServiceFactory.Should().NotBeSameAs(mainServiceFactory);

            var actualTestService = builder.Build(serviceProvider);

            actualTestService.Should().BeOfType<TestDecoratorService>()
                .Which.TestService.Should().BeSameAs(mainService);

            mockMainServiceFactory.Verify(m => m.Invoke(serviceProvider), Times.Once());

            mockMainServiceFactory.VerifyNoOtherCalls();
            mockServiceProvider.VerifyNoOtherCalls();
        }
    }
}
