
using System;
using System.ComponentModel;
using System.Web;

namespace Avantech.Common.ComponentModel.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class HubDisplayNameAttribute : DisplayNameAttribute
    {
        private const string ResourceType = "Property";
        private readonly string _resourceKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="HubDisplayNameAttribute"/> class.
        /// </summary>
        /// <param name="resourceKey">The resource key.</param>
        public HubDisplayNameAttribute(string resourceKey)
        {
            if (resourceKey == null) throw new ArgumentNullException("resourceKey");
            _resourceKey = resourceKey;
        }

        /// <summary>
        /// Gets the display name for a property that this attribute is placed against.
        /// </summary>
        /// <returns>The display name.</returns>
        public override string DisplayName
        {
            get
            {
                return HttpContext.GetGlobalResourceObject(ResourceType, _resourceKey) as string;
            }
        }
    }
}