using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Deinfes extension methods for adding decorated services to a service collection.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a service with a lifetime specified in <paramref name="lifetime"/> of the type
        /// specified in <typeparamref name="TService"/> with a factory specified in
        /// <paramref name="serviceFactory"/> to the specified
        /// <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="services">
        /// The <see cref="IServiceCollection"/> to add the service to.
        /// </param>
        /// <param name="serviceFactory">The factory that creates the service.</param>
        /// <param name="lifetime">The <see cref="ServiceLifetime"/> of the service.</param>
        /// <returns>
        /// A <see cref="IDecoratingBuilder{T}"/> used to decorate the service.
        /// </returns>
        public static IDecoratingBuilder<TService> AddDecorated<TService>(
            this IServiceCollection services,
            Func<IServiceProvider, TService> serviceFactory,
            ServiceLifetime lifetime)
            where TService : class
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));
            if (serviceFactory is null)
                throw new ArgumentNullException(nameof(serviceFactory));
            if (!Enum.IsDefined(typeof(ServiceLifetime), lifetime))
                throw new ArgumentOutOfRangeException(nameof(lifetime));

            var builder = new DecoratingBuilder<TService>(serviceFactory);
            services.Add(new ServiceDescriptor(typeof(TService), builder.Build, lifetime));
            return builder;
        }

        /// <summary>
        /// Adds a service with a lifetime specified in <paramref name="lifetime"/> of the type
        /// specified in <typeparamref name="TService"/> with a factory specified in
        /// <paramref name="serviceFactory"/> to the specified
        /// <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="services">
        /// The <see cref="IServiceCollection"/> to add the service to.
        /// </param>
        /// <param name="serviceFactory">The factory that creates the service.</param>
        /// <param name="lifetime">The <see cref="ServiceLifetime"/> of the service.</param>
        /// <returns>
        /// A <see cref="IDecoratingBuilder{T}"/> used to decorate the service.
        /// </returns>
        public static IDecoratingBuilder<TService> AddDecorated<TService>(
            this IServiceCollection services,
            Func<TService> serviceFactory,
            ServiceLifetime lifetime)
            where TService : class
        {
            if (serviceFactory is null)
                throw new ArgumentNullException(nameof(serviceFactory));

            return services.AddDecorated(_ => serviceFactory.Invoke(), lifetime);
        }

        /// <summary>
        /// Adds a service with a lifetime specified in <paramref name="lifetime"/> of the type
        /// specified in <typeparamref name="TService"/> with an implementation type specified in
        /// <typeparamref name="TImplementation"/> to the specified
        /// <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="services">
        /// The <see cref="IServiceCollection"/> to add the service to.
        /// </param>
        /// <param name="lifetime">The <see cref="ServiceLifetime"/> of the service.</param>
        /// <returns>
        /// A <see cref="IDecoratingBuilder{T}"/> used to decorate the service.
        /// </returns>
        public static IDecoratingBuilder<TService> AddDecorated<TService, TImplementation>(
            this IServiceCollection services,
            ServiceLifetime lifetime)
            where TService : class
            where TImplementation : TService =>
            services.AddDecorated<TService>(
                serviceProvider => ActivatorUtilities.CreateInstance<TImplementation>(serviceProvider),
                lifetime);

        /// <summary>
        /// Adds a transient service of the type specified in <typeparamref name="TService"/> with
        /// a factory specified in <paramref name="serviceFactory"/> to the specified
        /// <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="services">
        /// The <see cref="IServiceCollection"/> to add the service to.
        /// </param>
        /// <param name="serviceFactory">The factory that creates the service.</param>
        /// <returns>
        /// A <see cref="IDecoratingBuilder{T}"/> used to decorate the service.
        /// </returns>
        public static IDecoratingBuilder<TService> AddDecoratedTransient<TService>(
            this IServiceCollection services,
            Func<IServiceProvider, TService> serviceFactory)
            where TService : class =>
            services.AddDecorated(serviceFactory, ServiceLifetime.Transient);

        /// <summary>
        /// Adds a transient service of the type specified in <typeparamref name="TService"/> with
        /// a factory specified in <paramref name="serviceFactory"/> to the specified
        /// <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="services">
        /// The <see cref="IServiceCollection"/> to add the service to.
        /// </param>
        /// <param name="serviceFactory">The factory that creates the service.</param>
        /// <returns>
        /// A <see cref="IDecoratingBuilder{T}"/> used to decorate the service.
        /// </returns>
        public static IDecoratingBuilder<TService> AddDecoratedTransient<TService>(
            this IServiceCollection services,
            Func<TService> serviceFactory)
            where TService : class =>
            services.AddDecorated(serviceFactory, ServiceLifetime.Transient);

        /// <summary>
        /// Adds a transient service of the type specified in <typeparamref name="TService"/> with
        /// an implementation type specified in <typeparamref name="TImplementation"/> to the
        /// specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="services">
        /// The <see cref="IServiceCollection"/> to add the service to.
        /// </param>
        /// <returns>
        /// A <see cref="IDecoratingBuilder{T}"/> used to decorate the service.
        /// </returns>
        public static IDecoratingBuilder<TService> AddDecoratedTransient<TService, TImplementation>(this IServiceCollection services)
            where TService : class
            where TImplementation : TService =>
            services.AddDecorated<TService, TImplementation>(ServiceLifetime.Transient);

        /// <summary>
        /// Adds a scoped service of the type specified in <typeparamref name="TService"/> with
        /// a factory specified in <paramref name="serviceFactory"/> to the specified
        /// <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="services">
        /// The <see cref="IServiceCollection"/> to add the service to.
        /// </param>
        /// <param name="serviceFactory">The factory that creates the service.</param>
        /// <returns>
        /// A <see cref="IDecoratingBuilder{T}"/> used to decorate the service.
        /// </returns>
        public static IDecoratingBuilder<TService> AddDecoratedScoped<TService>(
            this IServiceCollection services,
            Func<IServiceProvider, TService> serviceFactory)
            where TService : class =>
            services.AddDecorated(serviceFactory, ServiceLifetime.Scoped);

        /// <summary>
        /// Adds a scoped service of the type specified in <typeparamref name="TService"/> with
        /// a factory specified in <paramref name="serviceFactory"/> to the specified
        /// <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="services">
        /// The <see cref="IServiceCollection"/> to add the service to.
        /// </param>
        /// <param name="serviceFactory">The factory that creates the service.</param>
        /// <returns>
        /// A <see cref="IDecoratingBuilder{T}"/> used to decorate the service.
        /// </returns>
        public static IDecoratingBuilder<TService> AddDecoratedScoped<TService>(
            this IServiceCollection services,
            Func<TService> serviceFactory)
            where TService : class =>
            services.AddDecorated(serviceFactory, ServiceLifetime.Scoped);

        /// <summary>
        /// Adds a scoped service of the type specified in <typeparamref name="TService"/> with
        /// an implementation type specified in <typeparamref name="TImplementation"/> to the
        /// specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="services">
        /// The <see cref="IServiceCollection"/> to add the service to.
        /// </param>
        /// <returns>
        /// A <see cref="IDecoratingBuilder{T}"/> used to decorate the service.
        /// </returns>
        public static IDecoratingBuilder<TService> AddDecoratedScoped<TService, TImplementation>(
            this IServiceCollection services)
            where TService : class
            where TImplementation : TService =>
            services.AddDecorated<TService, TImplementation>(ServiceLifetime.Scoped);

        /// <summary>
        /// Adds a singleton service of the type specified in <typeparamref name="TService"/> with
        /// a factory specified in <paramref name="serviceFactory"/> to the specified
        /// <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="services">
        /// The <see cref="IServiceCollection"/> to add the service to.
        /// </param>
        /// <param name="serviceFactory">The factory that creates the service.</param>
        /// <returns>
        /// A <see cref="IDecoratingBuilder{T}"/> used to decorate the service.
        /// </returns>
        public static IDecoratingBuilder<TService> AddDecoratedSingleton<TService>(
            this IServiceCollection services,
            Func<IServiceProvider, TService> serviceFactory)
            where TService : class =>
            services.AddDecorated(serviceFactory, ServiceLifetime.Singleton);

        /// <summary>
        /// Adds a singleton service of the type specified in <typeparamref name="TService"/> with
        /// a factory specified in <paramref name="serviceFactory"/> to the specified
        /// <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="services">
        /// The <see cref="IServiceCollection"/> to add the service to.
        /// </param>
        /// <param name="serviceFactory">The factory that creates the service.</param>
        /// <returns>
        /// A <see cref="IDecoratingBuilder{T}"/> used to decorate the service.
        /// </returns>
        public static IDecoratingBuilder<TService> AddDecoratedSingleton<TService>(
            this IServiceCollection services,
            Func<TService> serviceFactory)
            where TService : class =>
            services.AddDecorated(serviceFactory, ServiceLifetime.Singleton);

        /// <summary>
        /// Adds a singleton service of the type specified in <typeparamref name="TService"/> with
        /// an implementation type specified in <typeparamref name="TImplementation"/> to the
        /// specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="services">
        /// The <see cref="IServiceCollection"/> to add the service to.
        /// </param>
        /// <returns>
        /// A <see cref="IDecoratingBuilder{T}"/> used to decorate the service.
        /// </returns>
        public static IDecoratingBuilder<TService> AddDecoratedSingleton<TService, TImplementation>(
            this IServiceCollection services)
            where TService : class
            where TImplementation : TService =>
            services.AddDecorated<TService, TImplementation>(ServiceLifetime.Singleton);

        /// <summary>
        /// Adds a singleton service of the type specified in <typeparamref name="TService"/> with
        /// an instance specified in <paramref name="implementationInstance"/> to the specified
        /// <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="services">
        /// The <see cref="IServiceCollection"/> to add the service to.
        /// </param>
        /// <param name="implementationInstance">The instance of the service.</param>
        /// <returns>
        /// A <see cref="IDecoratingBuilder{T}"/> used to decorate the service.
        /// </returns>
        public static IDecoratingBuilder<TService> AddDecoratedSingleton<TService>(
            this IServiceCollection services,
            TService implementationInstance)
            where TService : class
        {
            if (implementationInstance is null)
                throw new ArgumentNullException(nameof(implementationInstance));

            return services.AddDecorated(_ => implementationInstance, ServiceLifetime.Singleton);
        }
    }
}
