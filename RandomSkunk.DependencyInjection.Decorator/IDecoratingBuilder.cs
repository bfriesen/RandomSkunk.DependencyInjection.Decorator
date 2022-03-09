using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Defines an interface for adding decorators to a registered service.
    /// </summary>
    /// <typeparam name="TService">The type of the service to decorate.</typeparam>
    public interface IDecoratingBuilder<TService>
        where TService : class
    {
        /// <summary>
        /// Adds the decorator specified in <paramref name="decoratorFactory"/> to the service.
        /// </summary>
        /// <param name="decoratorFactory">
        /// The method that creates the instance of the decorator. The first parameter to the
        /// factory is the object being decorated; the second parameter is a
        /// <see cref="IServiceProvider"/> used to resolve any dependencies needed to create the
        /// decorator.
        /// </param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        IDecoratingBuilder<TService> AddDecorator(Func<TService, IServiceProvider, TService> decoratorFactory);
    }
}
