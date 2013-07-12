using System.Collections.Generic;

namespace Avantech.Common.Practices.DependencyInjections
{
    public interface IDependencyResolver : System.Web.Mvc.IDependencyResolver {
		new object GetService(System.Type serviceType);

		new IEnumerable<object> GetServices(System.Type serviceType);
	}
}
