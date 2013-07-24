using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace Avantech.Common.Web.Mvc{
    public static class ControllerExtensions {
        public static RedirectToRouteResult RedirectToActionPermanent<TController>(this TController controller, Expression<Action<TController>> action, bool useTempData = false, string routeName = null) 
			where TController : Controller {

            return _RedirectToAction(controller, action, useTempData, true, routeName);
        }

        public static RedirectToRouteResult RedirectToAction<TController>(this TController controller, Expression<Action<TController>> action, bool useTempData = false, string routeName = null) 
			where TController : Controller {
            return _RedirectToAction(controller, action, useTempData, false, routeName);
        }

        private static RedirectToRouteResult _RedirectToAction<TController>(
            TController controller,
            Expression<Action<TController>> action,
            bool useTempData,
            bool permanent,
            string routeName)
            where TController : Controller {
            if (controller == null) {
                throw new ArgumentNullException("controller");
            }

            RouteValueDictionary routeValues = Microsoft.Web.Mvc.Internal.ExpressionHelper.GetRouteValuesFromExpression<TController>(action);

            if (useTempData)
                MoveActionParametersToTempData(routeValues, controller.TempData);

            return new RedirectToRouteResult(routeName, routeValues, permanent);
        }

        private static void MoveActionParametersToTempData(RouteValueDictionary routeValues, TempDataDictionary tempData) {
            tempData.Clear();

            List<KeyValuePair<string, object>> actionParameters = routeValues.Where(kv => !"Controller,Action,Area".Contains(kv.Key)).Select(kv => new KeyValuePair<string, object>(kv.Key, kv.Value)).ToList();

            actionParameters.ForEach(ap => {
                if (tempData.ContainsKey(ap.Key))
                    tempData[ap.Key] = ap.Value;
                else
                    tempData.Add(ap.Key, ap.Value);
                routeValues.Remove(ap.Key);
            });
        }    
    }
}
