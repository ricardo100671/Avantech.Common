

namespace Avantech.Common.Web.Mvc.Html
{
    /// <summary>
    /// Interface representing a rag bar item for use in ui displays.
    /// </summary>
    public interface IRagBarItem
    {
        /// <summary>
        /// Gets or sets the red count.
        /// </summary>
        int RedCount { get; set; }
        /// <summary>
        /// Gets or sets the amber count.
        /// </summary>
        int AmberCount { get; set; }
        /// <summary>
        /// Gets or sets the green count.
        /// </summary>
        int GreenCount { get; set; }
    }
}
