using System;
using System.ServiceModel;

namespace Avantech.Common.ServiceModel
{
    using Avantech.Common.Practices.ServiceLocation;
    using Avantech.Common.ServiceModel.Description;

    public class ServiceLocatorServiceHost : ServiceHost
    {
        private readonly ServiceLocatorBase _ServiceLocator;

        public ServiceLocatorServiceHost()
        { }

        public ServiceLocatorServiceHost(Type serviceType, ServiceLocatorBase serviceLocator, params Uri[] baseAddresses)
            : base(serviceLocator.GetServiceType(serviceType), baseAddresses)
        {
            _ServiceLocator = serviceLocator;
        }

        protected override void OnOpening()
        {
            Description.Behaviors.Add(new ServiceLocatorMapServiceBehavior(_ServiceLocator));
            base.OnOpening();
        }

    }
}
