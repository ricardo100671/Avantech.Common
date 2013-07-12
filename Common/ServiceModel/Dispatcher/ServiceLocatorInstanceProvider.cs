using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace Avantech.Common.ServiceModel.Dispatcher
{

    public class ServiceLocatorInstanceProvider : IInstanceProvider
    {
        private readonly Type _ServiceType;
        private readonly ServiceLocatorBase _ServiceLocator;

        public ServiceLocatorInstanceProvider(Type serviceType, ServiceLocatorBase serviceLocator)
        {
            _ServiceType = serviceType;
            _ServiceLocator = serviceLocator;
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return GetInstance(instanceContext, null);
        }

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            return _ServiceLocator.GetInstance(_ServiceType);
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
        }
    }
}
