using System;
using Microsoft.Practices.ServiceLocation;

namespace Avantech.Common.Practices.ServiceLocation
{
    using Avantech.Common.ServiceModel;

    /// <summary>
    /// Base class for implementing a custom Sevice locator with any dependendy injection container
    /// </summary>
    public abstract class ServiceLocatorBase : ServiceLocatorImplBase
    {
        /// <summary>
        /// Gets the type of a service for the specified service interface.
        /// <remarks>
        /// This is utilised by <see cref="ServiceLocatorServiceHost"/> in order to resolve WCF services, 
        /// when used used as the factory in WCF service(.svc) file.
        /// </remarks>
        /// </summary>
        /// <param name="interfaceType">Type of the service.</param>
        /// <returns></returns>
        public abstract Type GetServiceType(Type interfaceType);
    }
}
