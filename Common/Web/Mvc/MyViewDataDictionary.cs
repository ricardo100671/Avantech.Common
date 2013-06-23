namespace MySystem.Web.Mvc {
	using System.Web.Mvc;
	
	public class MyViewDataDictionary : ViewDataDictionary {
		MyTemplateInfo _TemplateInfo;

		public new MyTemplateInfo TemplateInfo {
			get {
				return _TemplateInfo
				       ?? (_TemplateInfo = new MyTemplateInfo());
			}
			set { _TemplateInfo = value; }
		}
	}

	public class MyViewDataDictionary<TModel> : ViewDataDictionary<TModel> {
		MyTemplateInfo _TemplateInfo;
		new public MyTemplateInfo TemplateInfo {
			get { 
				return _TemplateInfo 
					?? (_TemplateInfo = new MyTemplateInfo()); 
			}
			set
			{
				_TemplateInfo = value;
			}
		}
	}
}
