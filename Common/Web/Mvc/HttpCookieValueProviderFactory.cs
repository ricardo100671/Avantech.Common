
using System.Web.Mvc;

namespace Avantech.Common.Web.Mvc {
    public class HubHttpCookieValueProviderFactory : ValueProviderFactory {
		/// <summary>
        /// Instantiates and returns a Value Provider which retrieves values from Request Cookies
        /// </summary>
        /// <remarks>
		/// The provider relies on the naming convention of action parameters in order to locate the required cookie value, 
		/// where the name of the parameter must match the name of the required cookie and be suffixed with the words "HttpCookie",
        /// any special chaacters, which may be legal in a cookie name, but not legal in parameter names, such as hyphens '-', can simply be ommited.
        /// </remarks>
        /// <param name="controllerContext"></param>
        /// <returns></returns>
        public override IValueProvider GetValueProvider(ControllerContext controllerContext) {
            return new HttpCookieValueProvider(controllerContext.HttpContext.Request.Cookies);
        }
	}
}
