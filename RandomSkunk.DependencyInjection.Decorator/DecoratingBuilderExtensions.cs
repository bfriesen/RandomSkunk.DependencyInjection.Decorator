using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Deinfes extension methods for adding decorators to a registered service.
    /// </summary>
    public static class DecoratingBuilderExtensions
    {
        /// <summary>
        /// Adds the decorator specified in <paramref name="decoratorFactory"/> to the service.
        /// </summary>
        /// <typeparam name="TService">The type of the service being decorated.</typeparam>
        /// <param name="builder">
        /// The <see cref="IDecoratingBuilder{TService}"/> to add the decorator to.
        /// </param>
        /// <param name="decoratorFactory">
        /// The method that creates the instance of the decorator. The first parameter to the
        /// factory is the object being decorated; the second parameter is a
        /// <see cref="IServiceProvider"/> used to resolve any dependencies needed to create the
        /// decorator.
        /// </param>
        /// <returns>
        /// A reference to the <paramref name="builder"/> parameter after the operation has
        /// completed.
        /// </returns>
        public static IDecoratingBuilder<TService> AddDecorator<TService>(
            this IDecoratingBuilder<TService> builder,
            Func<TService, TService> decoratorFactory)
            where TService : class
        {
            if (decoratorFactory is null)
                throw new ArgumentNullException(nameof(decoratorFactory));

            return builder.AddDecorator((serviceToDecorate, _) => decoratorFactory.Invoke(serviceToDecorate));
        }

        /// <summary>
        /// Adds a decorator with an implementation type specified in
        /// <typeparamref name="TDecoratorImplementation"/> to the service.
        /// </summary>
        /// <typeparam name="TService">The type of the service being decorated.</typeparam>
        /// <typeparam name="TDecoratorImplementation">The type of the decorator implementation to use.</typeparam>
        /// <param name="builder">
        /// The <see cref="IDecoratingBuilder{TService}"/> to add the decorator to.
        /// </param>
        /// <returns>
        /// A reference to the <paramref name="builder"/> parameter after the operation has
        /// completed.
        /// </returns>
        public static IDecoratingBuilder<TService> AddDecorator<TService, TDecoratorImplementation>(this IDecoratingBuilder<TService> builder)
            where TService : class
            where TDecoratorImplementation : TService =>
            builder.AddDecorator((serviceToDecorate, serviceProvider) =>
                ActivatorUtilities.CreateInstance<TDecoratorImplementation>(serviceProvider, serviceToDecorate));
    }
}
