namespace MyLibrary.Collections.Generic {
	public interface ITreeDictionaryItem
	{
		int Id { get; set; }
		int? ParentId { get; set; }
		string Name { get; set; }
	}
}