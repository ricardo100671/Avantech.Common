namespace MyLibrary.Collections.Generic
{
	public class TreeDictionaryItem : ITreeDictionaryItem
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the parent id.
        /// </summary>
        /// <value>The parent id.</value>
        public int? ParentId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the display sequence.
        /// </summary>
        /// <value>The display sequence.</value>
        public int DisplaySequence { get; set; }

        /// <summary>
        /// Gets or sets the type of the icon.
        /// </summary>
        /// <value>The type of the icon.</value>
        public string IconType { get; set; }

        /// <summary>
        /// Gets or sets the entity id.
        /// </summary>
        /// <value>The entity id.</value>
        public int? EntityId { get; set; }

        /// <summary>
        /// Gets or sets the type of the entity.
        /// </summary>
        /// <value>The type of the entity.</value>
        public string EntityType { get; set; }

        public string Title { get; set; }
    }
}
