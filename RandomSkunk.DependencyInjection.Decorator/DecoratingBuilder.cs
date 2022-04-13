using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// An object that adds decorators to a registered service.
    /// </summary>
    /// <typeparam name="TService">The type of service to decorate.</typeparam>
    public class DecoratingBuilder<TService> : IDecoratingBuilder<TService>
        where TService : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DecoratingBuilder{TService}"/> class.
        /// </summary>
        /// <param name="serviceFactory">The factory that creates the service.</param>
        public DecoratingBuilder(Func<IServiceProvider, TService> serviceFactory)
        {
            ServiceFactory = serviceFactory ?? throw new ArgumentNullException(nameof(serviceFactory));
        }

        /// <summary>
        /// Gets the factory that creates the service.
        /// </summary>
        public Func<IServiceProvider, TService> ServiceFactory { get; private set; }

        /// <inheritdoc/>
        public IDecoratingBuilder<TService> AddDecorator(Func<TService, IServiceProvider, TService> decoratorFactory)
        {
            if (decoratorFactory is null)
                throw new ArgumentNullException(nameof(decoratorFactory));

            var serviceFactory = ServiceFactory;
            ServiceFactory = serviceProvider =>
                decoratorFactory.Invoke(serviceFactory.Invoke(serviceProvider), serviceProvider);

            return this;
        }

        /// <summary>
        /// Builds the decorated service.
        /// </summary>
        /// <param name="serviceProvider">
        /// The <see cref="IServiceProvider"/> used to resolve any dependencies needed to create
        /// the implementation.
        /// </param>
        /// <returns>The decorated service.</returns>
        public TService Build(IServiceProvider serviceProvider) => ServiceFactory.Invoke(serviceProvider);
    }
}
