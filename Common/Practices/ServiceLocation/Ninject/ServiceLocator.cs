using System;
using System.Collections.Generic;
using Ninject;
using Ninject.Parameters;

namespace Avantech.Common.Practices.ServiceLocation.Ninject
{
    public abstract class ServiceLocatorBase : ServiceLocation.ServiceLocatorBase, IDisposable
    {
        protected ServiceLocatorBase(object scope)
        {
            _Kernel = new StandardKernel();
            RegisterModules(Kernel, context);
        }

        readonly IKernel _Kernel;
        public IKernel Kernel {
            get { return _Kernel; }
        }

        public override Type GetServiceType(Type interfaceType)
        {
            return Kernel.GetServiceType(interfaceType);
        }

        protected override object DoGetInstance(Type serviceType, string key)
        {
            if (!string.IsNullOrEmpty(key))
                return Kernel.Get(serviceType, key, new IParameter[0]);

            return Kernel.TryGet(serviceType, (string)null, new IParameter[0]);
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            try
            {
                return Kernel.GetAll(serviceType, new IParameter[0]);
            }
            catch
            {
                return null;
            }
        }

        public void Dispose()
        {
            Kernel.Dispose();
        }

        public abstract void RegisterModules(IKernel kernel, object scope);
    }
}