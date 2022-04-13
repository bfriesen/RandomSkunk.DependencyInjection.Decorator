using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using Xunit;

namespace UnitTests
{
    public class DecoratingBuilderTests
    {
        [Fact(DisplayName = "Constructor sets ServiceFactory property from parameter")]
        public void ConstructorHappyPath()
        {
            var serviceFactory = new Mock<Func<IServiceProvider, ITestService>>().Object;

            var builder = new DecoratingBuilder<ITestService>(serviceFactory);

            builder.ServiceFactory.Should().BeSameAs(serviceFactory);
        }

        [Fact(DisplayName = "Constructor throws when serviceFactory is null")]
        public void ConstructorSadPath()
        {
            Action act = () => _ = new DecoratingBuilder<ITestService>(null!);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact(DisplayName = "Build method invokes serviceFactory")]
        public void BuildMethodHappyPath1()
        {
            var mainService = new Mock<ITestService>().Object;

            var mockMainServiceFactory = new Mock<Func<IServiceProvider, ITestService>>();
            mockMainServiceFactory.Setup(m => m.Invoke(It.IsAny<IServiceProvider>()))
                .Returns(mainService);
            var mainServiceFactory = mockMainServiceFactory.Object;

            var mockServiceProvider = new Mock<IServiceProvider>();
            var serviceProvider = mockServiceProvider.Object;

            var builder = new DecoratingBuilder<ITestService>(mainServiceFactory);

            var actualTestService = builder.Build(serviceProvider);

            actualTestService.Should().BeSameAs(mainService);

            mockMainServiceFactory.Verify(m => m.Invoke(serviceProvider), Times.Once());

            mockMainServiceFactory.VerifyNoOtherCalls();
            mockServiceProvider.VerifyNoOtherCalls();
        }

        [Fact(DisplayName = "Build method invokes serviceFactory then decoratorFactory when decorator has been added")]
        public void BuildMethodHappyPath2()
        {
            var mainService = new Mock<ITestService>().Object;
            var decoratorService = new Mock<ITestService>().Object;

            var mockMainServiceFactory = new Mock<Func<IServiceProvider, ITestService>>();
            mockMainServiceFactory.Setup(m => m.Invoke(It.IsAny<IServiceProvider>()))
                .Returns(mainService);
            var mainServiceFactory = mockMainServiceFactory.Object;

            var mockDecoratorFactory = new Mock<Func<ITestService, IServiceProvider, ITestService>>();
            mockDecoratorFactory.Setup(m => m.Invoke(It.IsAny<ITestService>(), It.IsAny<IServiceProvider>()))
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
            mockDecoratorFactory.Verify(m => m.Invoke(mainService, serviceProvider), Times.Once());

            mockMainServiceFactory.VerifyNoOtherCalls();
            mockDecoratorFactory.VerifyNoOtherCalls();
            mockServiceProvider.VerifyNoOtherCalls();
        }

        [Fact(DisplayName = "Build method invokes serviceFactory then each decoratorFactory when multiple decorators have been added")]
        public void BuildMethodHappyPath3()
        {
            var mainService = new Mock<ITestService>().Object;
            var firstDecoratorService = new Mock<ITestService>().Object;
            var middleDecoratorService = new Mock<ITestService>().Object;
            var lastDecoratorService = new Mock<ITestService>().Object;

            var mockMainServiceFactory = new Mock<Func<IServiceProvider, ITestService>>();
            mockMainServiceFactory.Setup(m => m.Invoke(It.IsAny<IServiceProvider>()))
                .Returns(mainService);
            var mainServiceFactory = mockMainServiceFactory.Object;

            var mockFirstDecoratorFactory = new Mock<Func<ITestService, IServiceProvider, ITestService>>();
            mockFirstDecoratorFactory.Setup(m => m.Invoke(It.IsAny<ITestService>(), It.IsAny<IServiceProvider>()))
                .Returns(firstDecoratorService);
            var firstDecoratorFactory = mockFirstDecoratorFactory.Object;

            var mockMiddleDecoratorFactory = new Mock<Func<ITestService, IServiceProvider, ITestService>>();
            mockMiddleDecoratorFactory.Setup(m => m.Invoke(It.IsAny<ITestService>(), It.IsAny<IServiceProvider>()))
                .Returns(middleDecoratorService);
            var middleDecoratorFactory = mockMiddleDecoratorFactory.Object;

            var mockLastDecoratorFactory = new Mock<Func<ITestService, IServiceProvider, ITestService>>();
            mockLastDecoratorFactory.Setup(m => m.Invoke(It.IsAny<ITestService>(), It.IsAny<IServiceProvider>()))
                .Returns(lastDecoratorService);
            var lastDecoratorFactory = mockLastDecoratorFactory.Object;

            var mockServiceProvider = new Mock<IServiceProvider>();
            var serviceProvider = mockServiceProvider.Object;

            var builder = new DecoratingBuilder<ITestService>(mainServiceFactory);
            builder.AddDecorator(firstDecoratorFactory);
            builder.AddDecorator(middleDecoratorFactory);
            builder.AddDecorator(lastDecoratorFactory);

            builder.ServiceFactory.Should().NotBeSameAs(mainServiceFactory);

            var actualTestService = builder.Build(serviceProvider);

            actualTestService.Should().BeSameAs(lastDecoratorService);

            mockMainServiceFactory.Verify(m => m.Invoke(serviceProvider), Times.Once());
            mockFirstDecoratorFactory.Verify(m => m.Invoke(mainService, serviceProvider), Times.Once());
            mockMiddleDecoratorFactory.Verify(m => m.Invoke(firstDecoratorService, serviceProvider), Times.Once());
            mockLastDecoratorFactory.Verify(m => m.Invoke(middleDecoratorService, serviceProvider), Times.Once());

            mockMainServiceFactory.VerifyNoOtherCalls();
            mockFirstDecoratorFactory.VerifyNoOtherCalls();
            mockMiddleDecoratorFactory.VerifyNoOtherCalls();
            mockLastDecoratorFactory.VerifyNoOtherCalls();
            mockServiceProvider.VerifyNoOtherCalls();
        }

        [Fact(DisplayName = "AddDecorator method throws when decoratorFactory is null")]
        public void AddDecoratorMethodSadPath()
        {
            var primaryService = new Mock<ITestService>().Object;

            var mockServiceFactory = new Mock<Func<IServiceProvider, ITestService>>();
            mockServiceFactory.Setup(m => m.Invoke(It.IsAny<IServiceProvider>()))
                .Returns(primaryService);
            var serviceFactory = mockServiceFactory.Object;

            var builder = new DecoratingBuilder<ITestService>(serviceFactory);

            Func<ITestService, IServiceProvider, ITestService>? decoratorFactory = null;

            Action act = () => builder.AddDecorator(decoratorFactory!);

            act.Should().Throw<ArgumentNullException>();
        }
    }
}
