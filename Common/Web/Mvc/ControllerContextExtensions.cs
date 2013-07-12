
using System.Web.Mvc;

namespace Avantech.Common.Web.Mvc
{
    public static class ControllerContextExtensions
    {

        /// <summary>
        ///   Gets the route string in the form '(/area)/controller/action'.
        /// </summary>
        /// <param name = "controllerContext">The controller context.</param>
        /// <returns></returns>
        public static string GetRouteString(this ControllerContext controllerContext)
        {
            var area = controllerContext.RouteData.DataTokens["area"] as string;
            var controller = controllerContext.RouteData.Values["controller"] as string;
            var action = controllerContext.RouteData.Values["actionOverride"] as string;
            if(string.IsNullOrEmpty(action))
                action = controllerContext.RouteData.Values["action"] as string;

            if (area == null)
                return string.Concat(controller, "/", action); //keep as string.concat for performance over string.format
            else
                return string.Concat(area, "/", controller, "/", action); //keep as string.concat for performance over string.format
        }
    }
}