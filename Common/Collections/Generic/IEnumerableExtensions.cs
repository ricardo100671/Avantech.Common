using Avantech.Common.Linq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Avantech.Common.Collections.Generic
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Retrieves a paged selection of records according to the parameters supplied.
        /// </summary>
        /// <typeparam name="T">Any class implementing IEntity</typeparam>
        /// <param name="source">The source collection.</param>
        /// <param name="skip">The number of records to skip.</param>
        /// <param name="take">The number of records to take.</param>
        /// <param name="search">The search string to be matched.</param>
        /// <param name="sortcol">The column name to apply sorting.</param>
        /// <param name="sortdir">The direction to sort in, use 'asc' to sort ascending, 'desc' to sort descending.</param>
        /// <param name="searchExpression">The search expression.</param>
        /// <returns></returns>
        public static IPagingResult<T> AsPagingResult<T>(this IEnumerable<T> source, int? skip, int? take, string search, string sortcol, string sortdir, Expression<Func<T, bool>> searchExpression) where T : class
        {
            var query = source.AsQueryable();
            var totalRecords = query.Count();

            if (skip.HasValue && take.HasValue && skip == totalRecords)
                skip = 0;

            // Filtering
            int totalFilteredRecords;
            if (searchExpression != null)
            {
                query = query.Where(searchExpression);

                totalFilteredRecords = query.Count();
            }
            else
                totalFilteredRecords = totalRecords;

            // Sorting
            sortcol = sortcol ?? "Id";
            sortdir = string.IsNullOrEmpty(sortdir) ? "desc" : sortdir.ToLower();
            query = sortdir == "desc" ? query.OrderByDescending(sortcol) : query.OrderBy(sortcol);

            // Paging
            if (!skip.HasValue || skip.Value < 0 || skip.Value > totalFilteredRecords)
                skip = 0;

            if (!take.HasValue || take.Value < 1)
                take = 10;

            query = query.Skip(skip.Value).Take(take.Value);

            return new PagingResult<T>(skip.Value, take.Value, search, sortcol, sortdir, totalRecords, totalFilteredRecords, query.ToList());
        }
    }
}
