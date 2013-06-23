namespace MyLibrary.Linq.Expressions
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;

	/// <summary>
	/// A class containing basic and inheritable functionality to a facilitate creation of a specialized
	/// data filter classes that can resolve filter names and associated values into linq expressions that can be used to retrieve data
	/// </summary>
	/// <typeparam name="TFilterTypesEnum">The enum type that lists the available filter types.</typeparam>
	/// <typeparam name="TExpressionRoot">The type of the expression's root.</typeparam>
	public abstract class DataFilterBase<TFilterTypesEnum, TExpressionRoot>
	{
		/// <summary>
		/// Used by consumer types to return a filter expression that can be used to select records, using a single keyword,
		/// for the purpose of performing a basic search where all filter types returned by <ref>BasicSearchFilterTypes</ref>
		/// are evaluated to contain the keyword.
		/// The filter also includes the filter returned by <ref>DataSecutiryFilter</ref>
		/// </summary>
		/// <param name="keyword">The filter value.</param>
		/// <returns></returns>
		public Expression<Func<TExpressionRoot, bool>> GetFilter(string keyword)
		{
			if (String.IsNullOrEmpty(keyword))
				return DefaultFilter;

			Expression<Func<TExpressionRoot, bool>> filter = e => false;

			var basicSearchFilter = BasicSearchFilterTypes
				.Aggregate(
					filter,
					(current, filterType) =>
						current.Or(
							ResolveFilterTypeAndValue(
								filterType,
								keyword
							)
						)
				);

			return DataSecurityFilter.And(DefaultFilter.And(basicSearchFilter));
		}

		/// <summary>
		/// Used by consumer types to returns a filter expression that can be used to select records, using a set of filter types and associated values,
		/// for the purpose of performing an advanced search where an expression is constructed by resolving each filter and it's associated values using  
		/// <ref>ResolveFilterTypeAndValue</ref>.
		/// The filter also includes the filter returned by <ref>DataSecutiryFilter</ref>
		/// </summary>
		/// <param name="filterDictionary">The filter dictionary.</param>
		/// <returns></returns>
		public Expression<Func<TExpressionRoot, bool>> GetFilter(Dictionary<TFilterTypesEnum, List<object>> filterDictionary)
		{
			if (!filterDictionary.Any())
				throw new ArgumentOutOfRangeException("filterDictionary");

			var filter = ResolveFilterTypeAndValues(
				filterDictionary.Keys.ElementAt(0),
				filterDictionary.Values.ElementAt(0)
			);

			filter = filterDictionary
				.Skip(1)
				.Aggregate(
					filter,
					(current, filterValuesEntry) =>
						current.And(
							ResolveFilterTypeAndValues(
								filterValuesEntry.Key,
								filterValuesEntry.Value
							)
						)
				);

			return DataSecurityFilter.And(DefaultFilter.And(filter));
		}

		/// <summary>
		/// Used by consumer types to retrun an default expression to filter records
		/// when no search criteria has been specified.
		/// </summary>
		/// <returns></returns>
		public virtual Expression<Func<TExpressionRoot, bool>> DefaultFilter
		{
			get { return f => true; }
		}

		/// <summary>
		/// Implemented by derived types to provide a filter expressions to filter records based on security information.
		/// </summary>
		protected abstract Expression<Func<TExpressionRoot, bool>> DataSecurityFilter { get; }

		/// <summary>
		/// Implemented by derived types to provide a list of filter types that will be used to construct an expression for the purpose of performing a basic search operation, 
		/// where a single keyword is evaluated against each filter member of the enumeration.
		/// </summary>
		protected abstract IEnumerable<TFilterTypesEnum> BasicSearchFilterTypes { get; }

		/// <summary>
		/// Utility method to resolve a filter type and associated values into valid expresions.
		/// </summary>
		/// <param name="filterTypes">The filter types.</param>
		/// <param name="values">The values.</param>
		/// <returns></returns>
		private Expression<Func<TExpressionRoot, bool>> ResolveFilterTypeAndValues(TFilterTypesEnum filterTypes, IEnumerable<object> values)
		{
			if (!values.Any())
				throw new ArgumentOutOfRangeException("values");

			var filter = ResolveFilterTypeAndValue(filterTypes, values.First());

			return values
					.Skip(1)
					.Aggregate(
						filter, 
						(current, value) => 
							current.Or(
								ResolveFilterTypeAndValue(
									filterTypes, 
									value
								)
							)
					);
		}

		/// <summary>
		/// Implemented by derived types to resolve a filterType and value into an appropriate filter expression.
		/// </summary>
		/// <param name="dataFilterType">The filter types.</param>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		protected abstract Expression<Func<TExpressionRoot, bool>> ResolveFilterTypeAndValue(TFilterTypesEnum dataFilterType, object value);
		
	}

	
}
