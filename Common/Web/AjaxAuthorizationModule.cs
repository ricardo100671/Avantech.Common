
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Avantech.Common.Web
{
    public class AjaxAuthorizationModule : IHttpModule
    {
        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpApplication"/> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application </param>
        public void Init(HttpApplication context)
        {
            context.PreSendRequestHeaders += CheckForAuthorizationFailure;
        }

        /// <summary>
        /// Checks for authorization failure.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CheckForAuthorizationFailure(object sender, EventArgs eventArgs)
        {
            var app = sender as HttpApplication;
            if (app == null) return;
            var response = new HttpResponseWrapper(app.Response);
            var request = new HttpRequestWrapper(app.Request);
            var context = new HttpContextWrapper(app.Context);
            CheckForAuthorizationFailure(request, response, context);
        }

        /// <summary>
        /// Checks for authorization failure and if result of ajax call overrides asp.net redirect to return a 401.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        /// <param name="context">The context.</param>
        internal void CheckForAuthorizationFailure(HttpRequestBase request, HttpResponseBase response, HttpContextBase context)
        {
            if (!request.IsAjaxRequest() || !true.Equals(context.Items["RequestWasNotAuthorized"])) return;
            response.StatusCode = 401;
            response.ClearContent();
            var content = new {title = HttpContext.GetGlobalResourceObject("Session", "Title.SessionHasExpired"), content = HttpContext.GetGlobalResourceObject("Session", "Messsage.SessionHasExpired")};
            var serializer = new JavaScriptSerializer();
            response.Write(serializer.Serialize(content));
        }

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule"/>.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
