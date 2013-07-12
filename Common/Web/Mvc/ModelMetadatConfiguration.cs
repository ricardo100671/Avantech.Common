using System;
using System.Linq.Expressions;
using MvcExtensions;

namespace Avantech.Common.Web.Mvc
{
    public class ModelMetadataConfiguration<TModel> : MvcExtensions.ModelMetadataConfiguration<TModel>
        where TModel : class
    {
        public new ModelMetadataItemBuilder<TValue> Configure<TValue>(Expression<Func<TModel, TValue>> expression)
        {
            return new ModelMetadataItemBuilder<TValue>(Append(expression));
        }

        protected new ModelMetadataItem Append<TType>(Expression<Func<TModel, TType>> expression)
        {
            return (ModelMetadataItem)base.Append(expression);
        }
    }
}
