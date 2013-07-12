using System;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace Avantech.Common.ServiceModel.Activition
{
    using Avantech.Common.Practices.ServiceLocation;

    /// <summary>
    /// A Service Host factory that utilises a Service Locator 
    /// in order to resolve a Service interface to a Service implementation.
    /// So that the Host can be decoupled from a service implementation
    /// </summary>
    public class ServiceLocatorServiceHostFactory : ServiceHostFactory
    {
        public static ServiceLocatorBase ServiceLocator { get; set; }

        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            if (ServiceLocator == null)
                throw new Exception("No ServiceLocator has been set");

            return new ServiceLocatorServiceHost(serviceType, ServiceLocator, baseAddresses);
        }
    }
}
