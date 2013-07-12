namespace Avantech.Common.Web.Mvc {
    public abstract class ViewPage : System.Web.Mvc.WebViewPage
	{
		ViewDataDictionary _ViewData;
		new public ViewDataDictionary ViewData
		{
			get
			{
				return _ViewData
					?? (_ViewData = new ViewDataDictionary());
			}
			set
			{
				_ViewData = value;
			}
		}
	}

    public abstract class ViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>
    {
		ViewDataDictionary<TModel> _ViewData;
		new public ViewDataDictionary<TModel> ViewData {
			get {
				return _ViewData 
					?? (_ViewData = new ViewDataDictionary<TModel>());
			} 
			set {
				_ViewData = value;
			}
		}
	}
}
