namespace MyLibrary.Collections.Generic {
	/// <summary>
	/// Represents an item that has hierarchical properties
	/// </summary>
	public interface IHierarchicalDictionaryItem
	{
		/// <summary>
		/// Gets or sets the key.
		/// </summary>
		string Key { get; set; }

		/// <summary>
		/// Gets or sets the parent key.
		/// </summary>
		string ParentKey { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		string Name { get; set; }
	}
}