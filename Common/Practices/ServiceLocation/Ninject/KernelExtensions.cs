using System;
using System.Collections.Generic;
using Ninject;

namespace Avantech.Common.Practices.ServiceLocation.Ninject
{
    using Avantech.Common.ServiceModel;

    public static class KernelExtension
    {
        private static readonly Dictionary<Type, Type> Services = new Dictionary<Type, Type>();

        /// <summary>
        /// Binds a service in a manner that it can be resolved be <see cref="ServiceLocatorServiceHost"/>.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="kernel">The kernel.</param>
        public static void BindService<TService, TImplementation>(this IKernel kernel) 
            where TImplementation : TService
        {
            kernel.Bind<TService>().To<TImplementation>();
            Services.Add(typeof(TService), typeof(TImplementation));
        }

        internal static Type GetServiceType(this IKernel kernel, Type interfaceType)
        {
            return Services[interfaceType];
        }
    }
}
