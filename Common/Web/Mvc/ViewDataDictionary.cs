namespace Avantech.Common.Web.Mvc {
    public class ViewDataDictionary : System.Web.Mvc.ViewDataDictionary
    {
		TemplateInfo _TemplateInfo;

		public new TemplateInfo TemplateInfo {
			get {
				return _TemplateInfo
				       ?? (_TemplateInfo = new TemplateInfo());
			}
			set { _TemplateInfo = value; }
		}
	}

	public class ViewDataDictionary<TModel> : System.Web.Mvc.ViewDataDictionary<TModel> {
		TemplateInfo _TemplateInfo;
		new public TemplateInfo TemplateInfo {
			get { 
				return _TemplateInfo 
					?? (_TemplateInfo = new TemplateInfo()); 
			}
			set
			{
				_TemplateInfo = value;
			}
		}
	}
}
