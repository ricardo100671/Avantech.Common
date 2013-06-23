namespace MyLibrary.Practices.DependencyInjections
{
	using System.Collections.Generic;
	using sys = System.Web.Mvc;

	public interface IDependencyResolver : sys.IDependencyResolver {
		new object GetService(System.Type serviceType);

		new IEnumerable<object> GetServices(System.Type serviceType);
	}
}
