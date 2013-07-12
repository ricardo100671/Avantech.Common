using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using System.Web.Mvc;

namespace Avantech.Common.ComponentModel.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true)]
    public class ValidationAttribute : System.ComponentModel.DataAnnotations.ValidationAttribute
    {
        private Type _validatorType;
        private string _method;
        private MethodInfo _methodInfo;
        private bool _isSingleArgumentMethod;
        private string _lastMessage;
        private Type _valuesType;
        private string _malformedErrorMessage;
        private Tuple<string, Type> _typeId;

        /// <summary>
        /// Gets the type that performs custom validation.
        /// </summary>
        /// 
        /// <returns>
        /// The type that performs custom validation.
        /// </returns>
        public Type ValidatorType
        {
            get
            {
                return _validatorType;
            }
        }

        /// <summary>
        /// Gets the validation method.
        /// </summary>
        /// 
        /// <returns>
        /// The name of the validation method.
        /// </returns>
        public string Method
        {
            get
            {
                return _method;
            }
        }

        /// <summary>
        /// Gets a unique identifier for this attribute.
        /// </summary>
        /// 
        /// <returns>
        /// The object that identifies this attribute.
        /// </returns>
        public override object TypeId
        {
            get
            {
                return _typeId ?? (_typeId = new Tuple<string, Type>(_method, _validatorType));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.ComponentModel.DataAnnotations.CustomValidationAttribute"/> class.
        /// </summary>
        /// <param name="validatorType">The type that contains the method that performs custom validation.</param><param name="method">The method that performs custom validation.</param>
        public ValidationAttribute(
            Type validatorType,
            string method
        )
            : base("Unexpected exception in 'Vanquis.NewBusiness.Shared.ComponentModel.DataAnnotations.CustomValidationAttribute'.")
        {
            _validatorType = validatorType;
            _method = method;
            _malformedErrorMessage = CheckAttributeWellFormed();
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ThrowIfAttributeNotWellFormed();

            MethodInfo methodInfo = _methodInfo;

            object convertedValue;

            if (!TryConvertValue(value, out convertedValue))
            {
                return new ValidationResult(
                    "Unable to convert value to type '{0}' for method '{1}' in '{2}'."
                    .FormatWith(
                        CultureInfo.CurrentCulture,
                        _valuesType,
                        _validatorType,
                        _method
                    )
                );
            }
            try
            {
                var objArray = !_isSingleArgumentMethod
                    ? new[] { convertedValue, validationContext }
                    : new[] { convertedValue };

                var parameters = objArray;

                var validatorTypeInstance = DependencyResolver.Current.GetService(_validatorType);

                var isValid = (bool)methodInfo.Invoke(validatorTypeInstance, parameters);

                ValidationResult validationResult = null;

                _lastMessage = null;

                if (!isValid)
                {
                    _lastMessage = ErrorMessage;
                    validationResult = new ValidationResult(ErrorMessage);
                }

                return validationResult;
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException != null) throw ex.InnerException;
                throw;
            }
        }

        /// <summary>
        /// Formats a validation error message.
        /// </summary>
        /// 
        /// <returns>
        /// An instance of the formatted error message.
        /// </returns>
        /// <param name="name">The name to include in the formatted message.</param>
        public override string FormatErrorMessage(string name)
        {
            ThrowIfAttributeNotWellFormed();
            if (string.IsNullOrEmpty(_lastMessage)) return base.FormatErrorMessage(name);

            return _lastMessage.FormatWith(
                CultureInfo.CurrentCulture,
                (object)name
            );
        }

        private string CheckAttributeWellFormed()
        {
            return ValidateValidatorTypeParameter() ?? ValidateMethodParameter();
        }

        private string ValidateValidatorTypeParameter()
        {
            if (_validatorType == null)
                return "ValidationType is required.";
            if (_validatorType.IsVisible)
                return null;
            return "'{0}' must be public.".FormatWith(_validatorType.Name);
        }

        /// <summary>
        /// Validates the method parameter.
        /// </summary>
        /// <returns></returns>
        private string ValidateMethodParameter()
        {
            if (string.IsNullOrEmpty(_method)) return "Method is required.";

            MethodInfo method = _validatorType.GetMethod(_method);

            if (method == null || !method.IsPublic)
            {
                return "'{0}' not found or not public in '{1}'.".FormatWith(_method, _validatorType.Name);
            }

            if (method.ReturnType != typeof(Boolean))
            {
                return "'{0}' in '{1}' must have a return type of 'System.Boolean'".FormatWith(_method, _validatorType.Name);
            }

            ParameterInfo[] parameters = method.GetParameters();
            if (parameters.Length == 0 ||
                parameters[0].ParameterType.IsByRef)
            {
                return "'{0}' in '{1}' has an invalid signature. '{0}' must have a first parameter type of 'System.Object' and an, optional, second parameter of type 'System.ComponentModel.DataAnnotations.ValidationResult'.".FormatWith(_method, _validatorType.Name);
            }

            _isSingleArgumentMethod = parameters.Length == 1;
            if (!_isSingleArgumentMethod
                && (
                    parameters.Length != 2
                    || parameters[1].ParameterType != typeof(ValidationContext)
                )
            )
            {
                return "'{0}' in '{1}' has an invalid signature. '{0}' must have a first parameter type of 'System.Object' and an, optional, second parameter of type 'System.ComponentModel.DataAnnotations.ValidationResult'.".FormatWith(_method, _validatorType.Name);
            }

            _methodInfo = method;
            _valuesType = parameters[0].ParameterType;

            return null;
        }

        private void ThrowIfAttributeNotWellFormed()
        {
            if (_malformedErrorMessage != null) throw new InvalidOperationException(_malformedErrorMessage);
        }

        private bool TryConvertValue(object value, out object convertedValue)
        {
            convertedValue = null;
            Type conversionType = _valuesType;
            if (value == null) return !conversionType.IsValueType || conversionType.IsGenericType && !(conversionType.GetGenericTypeDefinition() != typeof(Nullable<>));

            if (conversionType.IsInstanceOfType(value))
            {
                convertedValue = value;
                return true;
            }

            try
            {
                convertedValue = Convert.ChangeType(value, conversionType, (IFormatProvider)CultureInfo.CurrentCulture);
                return true;
            }
            catch (FormatException ex)
            {
                return false;
            }
            catch (InvalidCastException ex)
            {
                return false;
            }
            catch (NotSupportedException ex)
            {
                return false;
            }
        }
    }
}
