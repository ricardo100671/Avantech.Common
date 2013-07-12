using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Avantech.Common.ServiceModel.Description
{
    using Avantech.Common.Practices.ServiceLocation;
    using Avantech.Common.ServiceModel.Dispatcher;

    public class ServiceLocatorMapServiceBehavior : IServiceBehavior
    {
        private readonly ServiceLocatorBase _ServiceLocator;

        public ServiceLocatorMapServiceBehavior(ServiceLocatorBase serviceLocator)
        {
            _ServiceLocator = serviceLocator;
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcherBase cdb in serviceHostBase.ChannelDispatchers)
            {
                var cd = cdb as ChannelDispatcher;
                if (cd != null)
                {
                    foreach (EndpointDispatcher ed in cd.Endpoints)
                    {
                        ed.DispatchRuntime.InstanceProvider =
                            new ServiceLocatorInstanceProvider(serviceDescription.ServiceType, _ServiceLocator);
                    }
                }
            }
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        { }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        { }
    }
}
