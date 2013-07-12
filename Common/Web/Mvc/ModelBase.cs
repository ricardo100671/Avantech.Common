
using System;

namespace Avantech.Common.Web.Mvc {
    public class ModelBase<TController> {
		public Action<TController> Action { get; set; }
	}
}