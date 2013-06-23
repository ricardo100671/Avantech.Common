namespace MyLibrary.Web.Mvc
{
	using System.Web.Mvc;

	/// <summary>
    /// Attribute that provides additional information in RouteData for action override in retrieving local resources
    /// </summary>
    public class LocalisationActionAttribute : ActionFilterAttribute
    {
        private readonly string _actionOverride;
	    private readonly string _controllerOverride;
	    private readonly string _areaOverride;
        private const string ACTION_KEY = "actionOverride";
	    private const string CONTROLLER_KEY = "controllerOverride";
	    private const string AREA_KEY = "areaOverride";

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalisationActionAttribute"/> class.
        /// </summary>
        /// <param name="actionOverride">The action override value to be added to RouteData dictionary.</param>
        public LocalisationActionAttribute(string actionOverride)
        {
            _actionOverride = actionOverride;
            _controllerOverride = null;
            _areaOverride = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalisationActionAttribute"/> class.
        /// </summary>
        /// <param name="actionOverride">The action override value to be added to RouteData dictionary.</param>
        /// <param name="controllerOverride">The controller override value to be added to the RouteData dictionary.</param>
        public LocalisationActionAttribute(string actionOverride, string controllerOverride)
        {
            _actionOverride = actionOverride;
            _controllerOverride = controllerOverride;
            _areaOverride = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalisationActionAttribute"/> class.
        /// </summary>
        /// <param name="actionOverride">The action override value to be added to RouteData dictionary.</param>
        /// <param name="controllerOverride">The controller override value to be added to the RouteData dictionary.</param>
        /// <param name="areaOverride">The area override value to be added to the RouteData dictionary.</param>
        public LocalisationActionAttribute(string actionOverride, string controllerOverride, string areaOverride)
        {
            _actionOverride = actionOverride;
            _controllerOverride = controllerOverride;
            _areaOverride = areaOverride;
        }

        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(!filterContext.RouteData.Values.ContainsKey(ACTION_KEY))
                filterContext.RouteData.Values.Add(ACTION_KEY, _actionOverride);

            if (!filterContext.RouteData.Values.ContainsKey(CONTROLLER_KEY))
                filterContext.RouteData.Values.Add(CONTROLLER_KEY, _controllerOverride);

            if (!filterContext.RouteData.Values.ContainsKey(AREA_KEY))
                filterContext.RouteData.Values.Add(AREA_KEY, _areaOverride);

            base.OnActionExecuting(filterContext);
        }
    }
}
