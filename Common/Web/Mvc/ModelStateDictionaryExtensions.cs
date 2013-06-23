namespace MyLibrary.Web.Mvc
{
	using System;
	using System.Web.Mvc;
	using System.Linq;
	using System.Linq.Expressions;
	using Linq.Expressions;

	public static class ModelStateDictionaryExtensions
	{
		public static void Remove<TModel, TProperty>(
			this ModelStateDictionary thisModelStateDictionary,
			Expression<Func<TModel, TProperty>> expression
		)
		{
			var propertyName = expression.MemberName();

			thisModelStateDictionary.TakeWhile(ms =>
				ms.Key.StartsWith(propertyName)
				)
				.ToList()
				.ForEach(ms => thisModelStateDictionary.Remove(ms));
		}

	}
}
