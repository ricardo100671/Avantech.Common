namespace MyLibrary.Collections.Generic
{
	using System.Collections.Generic;

	public interface IPagingResult<T> where T : class
    {
        int Skip { get; }
        int Take { get; }
        string Search { get; }
        string SortColumn { get; }
        string SortDirection { get; }
        int TotalRecords { get; }
        int TotalFilteredRecords { get; }
        ICollection<T> Records { get; }
    }
}