
using System.Web;
using System.Web.Mvc;

namespace Avantech.Common.Web
{
    public static class HttpContextExtensions
    {
        /// <summary>
        ///   Gets the route string in the form '(/area)/controller/action'.
        /// </summary>
        /// <param name = "httpContext">The current HTTP context.</param>
        /// <returns>A string representing the mvc route information</returns>
        public static string GetRouteString(this HttpContext httpContext)
        {
            var handler = HttpContext.Current.Handler as MvcHandler;

            if (handler == null) return string.Empty;

            var area = handler.RequestContext.RouteData.Values["areaOverride"] as string;
            if (string.IsNullOrEmpty(area))
                area = handler.RequestContext.RouteData.DataTokens["area"] as string;

            var controller = handler.RequestContext.RouteData.Values["controllerOverride"] as string;
            if (string.IsNullOrEmpty(controller))
                controller = handler.RequestContext.RouteData.Values["controller"] as string;

            var action = handler.RequestContext.RouteData.Values["actionOverride"] as string;
            if (string.IsNullOrEmpty(action))
                action = handler.RequestContext.RouteData.Values["action"] as string;

            if (area == null)
                return string.Concat(controller, "/", action); //keep as string.concat for performance over string.format
            else
                return string.Concat(area, "/", controller, "/", action); //keep as string.concat for performance over string.format

        }
    }
}
