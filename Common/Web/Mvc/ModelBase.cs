namespace MySystem.Web.Mvc {
	using System;

	public class ModelBase<TController> {
		public Action<TController> Action { get; set; }
	}
}