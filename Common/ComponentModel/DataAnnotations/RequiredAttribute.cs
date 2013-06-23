namespace MyLibrary.ComponentModel.DataAnnotations
{
	using System;
	using System.ComponentModel.DataAnnotations;

	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class RequiredAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "Value Is Required";

        public RequiredAttribute()
            : base(DefaultErrorMessage)
        {
        }

        /// <summary>
        /// Determines whether the specified value of the object is valid.
        /// </summary>
        /// <param name="value">The value of the object to validate.</param>
        /// <returns>
        /// true if the specified value is valid; otherwise, false.
        /// </returns>
        public override bool IsValid(object value)
        {
            if (value != null)
                return value.ToString().Length > 0;
            
                return false;
        }
    }
}