using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Reflection;
using Xunit;

namespace UnitTests
{
    public class ServiceCollectionExtensionsTests
    {
        private static readonly MethodInfo _buildMethod = typeof(DecoratingBuilder<ITestService>).GetMethod(nameof(DecoratingBuilder<ITestService>.Build))!;

        [Fact(DisplayName = "AddDecorated extension method 1 adds service descriptor to service collection")]
        public void AddDecoratedExtensionMethod1HappyPath()
        {
            var services = new ServiceCollection();

            var mainService = new Mock<ITestService>().Object;

            var mockMainServiceFactory = new Mock<Func<IServiceProvider, ITestService>>();
            mockMainServiceFactory.Setup(m => m.Invoke(It.IsAny<IServiceProvider>()))
                .Returns(mainService);
            var mainServiceFactory = mockMainServiceFactory.Object;

            var lifetime = ServiceLifetime.Scoped;

            var builder = services.AddDecorated(mainServiceFactory, lifetime);

            builder.Should().BeOfType<DecoratingBuilder<ITestService>>()
                .Which.ServiceFactory.Should().BeSameAs(mainServiceFactory);

            var descriptor =
                services.Should().ContainSingle()
                    .Subject;
            descriptor.ServiceType.Should().Be<ITestService>();
            descriptor.Lifetime.Should().Be(lifetime);
            descriptor.ImplementationFactory.Should().NotBeNull();
            descriptor.ImplementationFactory!.Method.Should().BeSameAs(_buildMethod);
            descriptor.ImplementationFactory.Target.Should().BeSameAs(builder);
        }

        [Fact(DisplayName = "AddDecorated extension method 1 throws when services is null")]
        public void AddDecoratedExtensionMethod1SadPath1()
        {
            IServiceCollection? services = null;

            var mainService = new Mock<ITestService>().Object;

            var mockMainServiceFactory = new Mock<Func<IServiceProvider, ITestService>>();
            mockMainServiceFactory.Setup(m => m.Invoke(It.IsAny<IServiceProvider>()))
                .Returns(mainService);
            var mainServiceFactory = mockMainServiceFactory.Object;

            var lifetime = ServiceLifetime.Scoped;

            Action act = () => _ = services!.AddDecorated(mainServiceFactory, lifetime);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact(DisplayName = "AddDecorated extension method 1 throws when serviceFactory is null")]
        public void AddDecoratedExtensionMethod1SadPath2()
        {
            var services = new ServiceCollection();

            Func<IServiceProvider, ITestService>? mainServiceFactory = null;

            var lifetime = ServiceLifetime.Scoped;

            Action act = () => _ = services.AddDecorated(mainServiceFactory!, lifetime);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact(DisplayName = "AddDecorated extension method 1 throws when lifetime is not defined")]
        public void AddDecoratedExtensionMethod1SadPath3()
        {
            var services = new ServiceCollection();

            var mainService = new Mock<ITestService>().Object;

            var mockMainServiceFactory = new Mock<Func<IServiceProvider, ITestService>>();
            mockMainServiceFactory.Setup(m => m.Invoke(It.IsAny<IServiceProvider>()))
                .Returns(mainService);
            var mainServiceFactory = mockMainServiceFactory.Object;

            var lifetime = (ServiceLifetime)(-12345);

            Action act = () => _ = services.AddDecorated(mainServiceFactory, lifetime);

            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact(DisplayName = "AddDecorated extension method 2 adds service descriptor to service collection")]
        public void AddDecoratedExtensionMethod2HappyPath()
        {
            var services = new ServiceCollection();

            var mainService = new Mock<ITestService>().Object;

            var mockMainServiceFactory = new Mock<Func<ITestService>>();
            mockMainServiceFactory.Setup(m => m.Invoke())
                .Returns(mainService);
            var mainServiceFactory = mockMainServiceFactory.Object;

            var lifetime = ServiceLifetime.Scoped;

            var builder = services.AddDecorated(mainServiceFactory, lifetime);

            var decoratingBuilder =
                builder.Should().BeOfType<DecoratingBuilder<ITestService>>()
                    .Subject;

            var serviceProvider = new Mock<IServiceProvider>().Object;

            var service = decoratingBuilder.Build(serviceProvider);

            service.Should().BeSameAs(mainService);

            var descriptor =
                services.Should().ContainSingle()
                    .Subject;
            descriptor.ServiceType.Should().Be<ITestService>();
            descriptor.Lifetime.Should().Be(lifetime);
            descriptor.ImplementationFactory.Should().NotBeNull();
            descriptor.ImplementationFactory!.Method.Should().BeSameAs(_buildMethod);
            descriptor.ImplementationFactory.Target.Should().BeSameAs(builder);
        }

        [Fact(DisplayName = "AddDecorated extension method 2 throws when serviceFactory is null")]
        public void AddDecoratedExtensionMethod2SadPath()
        {
            var services = new ServiceCollection();

            var mainService = new Mock<ITestService>().Object;

            Func<ITestService>? mainServiceFactory = null;

            var lifetime = ServiceLifetime.Scoped;

            Action act = () => _ = services.AddDecorated(mainServiceFactory!, lifetime);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact(DisplayName = "AddDecorated extension method 3 adds service descriptor to service collection")]
        public void AddDecoratedExtensionMethod3HappyPath()
        {
            var services = new ServiceCollection();

            var lifetime = ServiceLifetime.Scoped;

            var builder = services.AddDecorated<ITestService, TestService>(lifetime);

            var decoratingBuilder =
                builder.Should().BeOfType<DecoratingBuilder<ITestService>>()
                    .Subject;

            var serviceProvider = new Mock<IServiceProvider>().Object;

            var service = decoratingBuilder.Build(serviceProvider);

            service.Should().BeOfType<TestService>();

            var descriptor =
                services.Should().ContainSingle()
                    .Subject;
            descriptor.ServiceType.Should().Be<ITestService>();
            descriptor.Lifetime.Should().Be(lifetime);
            descriptor.ImplementationFactory.Should().NotBeNull();
            descriptor.ImplementationFactory!.Method.Should().BeSameAs(_buildMethod);
            descriptor.ImplementationFactory.Target.Should().BeSameAs(builder);
        }

        [Fact(DisplayName = "AddDecoratedTransient extension method 1 adds service descriptor to service collection")]
        public void AddDecoratedTransientExtensionMethod1HappyPath()
        {
            var services = new ServiceCollection();

            var mainService = new Mock<ITestService>().Object;

            var mockMainServiceFactory = new Mock<Func<IServiceProvider, ITestService>>();
            mockMainServiceFactory.Setup(m => m.Invoke(It.IsAny<IServiceProvider>()))
                .Returns(mainService);
            var mainServiceFactory = mockMainServiceFactory.Object;

            var builder = services.AddDecoratedTransient(mainServiceFactory);

            builder.Should().BeOfType<DecoratingBuilder<ITestService>>()
                .Which.ServiceFactory.Should().BeSameAs(mainServiceFactory);

            var descriptor =
                services.Should().ContainSingle()
                    .Subject;
            descriptor.ServiceType.Should().Be<ITestService>();
            descriptor.Lifetime.Should().Be(ServiceLifetime.Transient);
            descriptor.ImplementationFactory.Should().NotBeNull();
            descriptor.ImplementationFactory!.Method.Should().BeSameAs(_buildMethod);
            descriptor.ImplementationFactory.Target.Should().BeSameAs(builder);
        }

        [Fact(DisplayName = "AddDecoratedTransient extension method 2 adds service descriptor to service collection")]
        public void AddDecoratedTransientExtensionMethod2HappyPath()
        {
            var services = new ServiceCollection();

            var mainService = new Mock<ITestService>().Object;

            var mockMainServiceFactory = new Mock<Func<ITestService>>();
            mockMainServiceFactory.Setup(m => m.Invoke())
                .Returns(mainService);
            var mainServiceFactory = mockMainServiceFactory.Object;

            var builder = services.AddDecoratedTransient(mainServiceFactory);

            var decoratingBuilder =
                builder.Should().BeOfType<DecoratingBuilder<ITestService>>()
                    .Subject;

            var serviceProvider = new Mock<IServiceProvider>().Object;

            var service = decoratingBuilder.Build(serviceProvider);

            service.Should().BeSameAs(mainService);

            var descriptor =
                services.Should().ContainSingle()
                    .Subject;
            descriptor.ServiceType.Should().Be<ITestService>();
            descriptor.Lifetime.Should().Be(ServiceLifetime.Transient);
            descriptor.ImplementationFactory.Should().NotBeNull();
            descriptor.ImplementationFactory!.Method.Should().BeSameAs(_buildMethod);
            descriptor.ImplementationFactory.Target.Should().BeSameAs(builder);
        }

        [Fact(DisplayName = "AddDecoratedTransient extension method 3 adds service descriptor to service collection")]
        public void AddDecoratedTransientExtensionMethod3HappyPath()
        {
            var services = new ServiceCollection();

            var builder = services.AddDecoratedTransient<ITestService, TestService>();

            var decoratingBuilder =
                builder.Should().BeOfType<DecoratingBuilder<ITestService>>()
                    .Subject;

            var serviceProvider = new Mock<IServiceProvider>().Object;

            var service = decoratingBuilder.Build(serviceProvider);

            service.Should().BeOfType<TestService>();

            var descriptor =
                services.Should().ContainSingle()
                    .Subject;
            descriptor.ServiceType.Should().Be<ITestService>();
            descriptor.Lifetime.Should().Be(ServiceLifetime.Transient);
            descriptor.ImplementationFactory.Should().NotBeNull();
            descriptor.ImplementationFactory!.Method.Should().BeSameAs(_buildMethod);
            descriptor.ImplementationFactory.Target.Should().BeSameAs(builder);
        }

        [Fact(DisplayName = "AddDecoratedScoped extension method 1 adds service descriptor to service collection")]
        public void AddDecoratedScopedExtensionMethod1HappyPath()
        {
            var services = new ServiceCollection();

            var mainService = new Mock<ITestService>().Object;

            var mockMainServiceFactory = new Mock<Func<IServiceProvider, ITestService>>();
            mockMainServiceFactory.Setup(m => m.Invoke(It.IsAny<IServiceProvider>()))
                .Returns(mainService);
            var mainServiceFactory = mockMainServiceFactory.Object;

            var builder = services.AddDecoratedScoped(mainServiceFactory);

            builder.Should().BeOfType<DecoratingBuilder<ITestService>>()
                .Which.ServiceFactory.Should().BeSameAs(mainServiceFactory);

            var descriptor =
                services.Should().ContainSingle()
                    .Subject;
            descriptor.ServiceType.Should().Be<ITestService>();
            descriptor.Lifetime.Should().Be(ServiceLifetime.Scoped);
            descriptor.ImplementationFactory.Should().NotBeNull();
            descriptor.ImplementationFactory!.Method.Should().BeSameAs(_buildMethod);
            descriptor.ImplementationFactory.Target.Should().BeSameAs(builder);
        }

        [Fact(DisplayName = "AddDecoratedScoped extension method 2 adds service descriptor to service collection")]
        public void AddDecoratedScopedExtensionMethod2HappyPath()
        {
            var services = new ServiceCollection();

            var mainService = new Mock<ITestService>().Object;

            var mockMainServiceFactory = new Mock<Func<ITestService>>();
            mockMainServiceFactory.Setup(m => m.Invoke())
                .Returns(mainService);
            var mainServiceFactory = mockMainServiceFactory.Object;

            var builder = services.AddDecoratedScoped(mainServiceFactory);

            var decoratingBuilder =
                builder.Should().BeOfType<DecoratingBuilder<ITestService>>()
                    .Subject;

            var serviceProvider = new Mock<IServiceProvider>().Object;

            var service = decoratingBuilder.Build(serviceProvider);

            service.Should().BeSameAs(mainService);

            var descriptor =
                services.Should().ContainSingle()
                    .Subject;
            descriptor.ServiceType.Should().Be<ITestService>();
            descriptor.Lifetime.Should().Be(ServiceLifetime.Scoped);
            descriptor.ImplementationFactory.Should().NotBeNull();
            descriptor.ImplementationFactory!.Method.Should().BeSameAs(_buildMethod);
            descriptor.ImplementationFactory.Target.Should().BeSameAs(builder);
        }

        [Fact(DisplayName = "AddDecoratedScoped extension method 3 adds service descriptor to service collection")]
        public void AddDecoratedScopedExtensionMethod3HappyPath()
        {
            var services = new ServiceCollection();

            var builder = services.AddDecoratedScoped<ITestService, TestService>();

            var decoratingBuilder =
                builder.Should().BeOfType<DecoratingBuilder<ITestService>>()
                    .Subject;

            var serviceProvider = new Mock<IServiceProvider>().Object;

            var service = decoratingBuilder.Build(serviceProvider);

            service.Should().BeOfType<TestService>();

            var descriptor =
                services.Should().ContainSingle()
                    .Subject;
            descriptor.ServiceType.Should().Be<ITestService>();
            descriptor.Lifetime.Should().Be(ServiceLifetime.Scoped);
            descriptor.ImplementationFactory.Should().NotBeNull();
            descriptor.ImplementationFactory!.Method.Should().BeSameAs(_buildMethod);
            descriptor.ImplementationFactory.Target.Should().BeSameAs(builder);
        }

        [Fact(DisplayName = "AddDecoratedSingleton extension method 1 adds service descriptor to service collection")]
        public void AddDecoratedSingletonExtensionMethod1HappyPath()
        {
            var services = new ServiceCollection();

            var mainService = new Mock<ITestService>().Object;

            var mockMainServiceFactory = new Mock<Func<IServiceProvider, ITestService>>();
            mockMainServiceFactory.Setup(m => m.Invoke(It.IsAny<IServiceProvider>()))
                .Returns(mainService);
            var mainServiceFactory = mockMainServiceFactory.Object;

            var builder = services.AddDecoratedSingleton(mainServiceFactory);

            builder.Should().BeOfType<DecoratingBuilder<ITestService>>()
                .Which.ServiceFactory.Should().BeSameAs(mainServiceFactory);

            var descriptor =
                services.Should().ContainSingle()
                    .Subject;
            descriptor.ServiceType.Should().Be<ITestService>();
            descriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
            descriptor.ImplementationFactory.Should().NotBeNull();
            descriptor.ImplementationFactory!.Method.Should().BeSameAs(_buildMethod);
            descriptor.ImplementationFactory.Target.Should().BeSameAs(builder);
        }

        [Fact(DisplayName = "AddDecoratedSingleton extension method 2 adds service descriptor to service collection")]
        public void AddDecoratedSingletonExtensionMethod2HappyPath()
        {
            var services = new ServiceCollection();

            var mainService = new Mock<ITestService>().Object;

            var mockMainServiceFactory = new Mock<Func<ITestService>>();
            mockMainServiceFactory.Setup(m => m.Invoke())
                .Returns(mainService);
            var mainServiceFactory = mockMainServiceFactory.Object;

            var builder = services.AddDecoratedSingleton(mainServiceFactory);

            var decoratingBuilder =
                builder.Should().BeOfType<DecoratingBuilder<ITestService>>()
                    .Subject;

            var serviceProvider = new Mock<IServiceProvider>().Object;

            var service = decoratingBuilder.Build(serviceProvider);

            service.Should().BeSameAs(mainService);

            var descriptor =
                services.Should().ContainSingle()
                    .Subject;
            descriptor.ServiceType.Should().Be<ITestService>();
            descriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
            descriptor.ImplementationFactory.Should().NotBeNull();
            descriptor.ImplementationFactory!.Method.Should().BeSameAs(_buildMethod);
            descriptor.ImplementationFactory.Target.Should().BeSameAs(builder);
        }

        [Fact(DisplayName = "AddDecoratedSingleton extension method 3 adds service descriptor to service collection")]
        public void AddDecoratedSingletonExtensionMethod3HappyPath()
        {
            var services = new ServiceCollection();

            var builder = services.AddDecoratedSingleton<ITestService, TestService>();

            var decoratingBuilder =
                builder.Should().BeOfType<DecoratingBuilder<ITestService>>()
                    .Subject;

            var serviceProvider = new Mock<IServiceProvider>().Object;

            var service = decoratingBuilder.Build(serviceProvider);

            service.Should().BeOfType<TestService>();

            var descriptor =
                services.Should().ContainSingle()
                    .Subject;
            descriptor.ServiceType.Should().Be<ITestService>();
            descriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
            descriptor.ImplementationFactory.Should().NotBeNull();
            descriptor.ImplementationFactory!.Method.Should().BeSameAs(_buildMethod);
            descriptor.ImplementationFactory.Target.Should().BeSameAs(builder);
        }

        [Fact(DisplayName = "AddDecoratedSingleton extension method 4 adds service descriptor to service collection")]
        public void AddDecoratedSingletonExtensionMethod4HappyPath()
        {
            var services = new ServiceCollection();

            var implementationServce = new Mock<ITestService>().Object;

            var builder = services.AddDecoratedSingleton(implementationServce);

            var decoratingBuilder =
                builder.Should().BeOfType<DecoratingBuilder<ITestService>>()
                    .Subject;

            var serviceProvider = new Mock<IServiceProvider>().Object;

            var service = decoratingBuilder.Build(serviceProvider);

            service.Should().BeSameAs(implementationServce);

            var descriptor =
                services.Should().ContainSingle()
                    .Subject;
            descriptor.ServiceType.Should().Be<ITestService>();
            descriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
            descriptor.ImplementationFactory.Should().NotBeNull();
            descriptor.ImplementationFactory!.Method.Should().BeSameAs(_buildMethod);
            descriptor.ImplementationFactory.Target.Should().BeSameAs(builder);
        }
    }
}
