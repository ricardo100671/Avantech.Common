
using System;
using System.Collections.Generic;

namespace Avantech.Common.Collections.Generic
{
    /// <summary>
    /// Represents a paged set of records containing navigation, count, sort and filter information for grid displays
    /// </summary>
    /// <typeparam name="T">Any class implementing IEntity</typeparam>
    public class PagingResult<T> : IPagingResult<T> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagingResult&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="skip">The number of records to skip.</param>
        /// <param name="take">The number of records to take.</param>
        /// <param name="search">The search string to be matched.</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="sortDirection">The sort direction.</param>
        /// <param name="totalRecords">The total records.</param>
        /// <param name="totalFilteredRecords">The total filtered records.</param>
        /// <param name="records">The resulting records.</param>
        public PagingResult(int skip, int take, string search, string sortColumn, string sortDirection, int totalRecords, int totalFilteredRecords, ICollection<T> records)
        {
            Skip = skip;
            Take = take;
            Search = search;
            SortColumn = sortColumn;
            SortDirection = sortDirection;
            TotalRecords = totalRecords;
            TotalFilteredRecords = totalFilteredRecords;
            Records = records;
        }

        #region IPagingResult<T> Members

        /// <summary>
        /// Gets the number of records to skip.
        /// </summary>
        public int Skip { get; private set; }

        /// <summary>
        /// Gets the number of records to take.
        /// </summary>
        public int Take { get; private set; }

        /// <summary>
        /// Gets the search string to be matched
        /// </summary>
        public string Search { get; private set; }

        /// <summary>
        /// Gets or sets the sort column.
        /// </summary>
        public string SortColumn { get; set; }

        /// <summary>
        /// Gets or sets the sort direction.
        /// </summary>
        public string SortDirection { get; set; }

        /// <summary>
        /// Gets the total records.
        /// </summary>
        public Int32 TotalRecords { get; private set; }

        /// <summary>
        /// Gets the total filtered records.
        /// </summary>
        public Int32 TotalFilteredRecords { get; private set; }

        /// <summary>
        /// Gets the records.
        /// </summary>
        public ICollection<T> Records { get; private set; }

        #endregion
    }
}
