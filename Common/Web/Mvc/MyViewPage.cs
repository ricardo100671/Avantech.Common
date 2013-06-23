namespace MySystem.Web.Mvc {
	using System.Web.Mvc;

	public abstract class MyViewPage : WebViewPage
	{
		MyViewDataDictionary _ViewData;
		new public MyViewDataDictionary ViewData
		{
			get
			{
				return _ViewData
					?? (_ViewData = new MyViewDataDictionary());
			}
			set
			{
				_ViewData = value;
			}
		}
	}

	public abstract class MyViewPage<TModel> : WebViewPage<TModel>	 {
		MyViewDataDictionary<TModel> _ViewData;
		new public MyViewDataDictionary<TModel> ViewData {
			get {
				return _ViewData 
					?? (_ViewData = new MyViewDataDictionary<TModel>());
			} 
			set {
				_ViewData = value;
			}
		}
	}
}
