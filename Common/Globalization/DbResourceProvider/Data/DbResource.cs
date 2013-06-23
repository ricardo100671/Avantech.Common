namespace MyLibrary.Globalization.DbResourceProvider.Data
{
	using System;

	internal class DbResource
    {
        /// <summary>
        /// Gets or sets the type of the resource.
        /// </summary>
        public string ResourceType { get; set; }

        /// <summary>
        /// Gets or sets the culture code.
        /// </summary>
        public string CultureCode { get; set; }

        /// <summary>
        /// Gets or sets the resource key.
        /// </summary>
        public string ResourceKey { get; set; }

        /// <summary>
        /// Gets or sets the resource value.
        /// </summary>
        public string ResourceValue { get; set; }

        /// <summary>
        /// Gets or sets the last updated date.
        /// </summary>
        public DateTime LastUpdatedDate { get; set; }
    }
}
