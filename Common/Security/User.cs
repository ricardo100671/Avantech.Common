
using Avantech.Common.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Avantech.Common.Security
{
    public abstract class User<TUser, TSecurityRole, TUserPasswordHistory, TEntityTypesEnum> : IAuditable<TEntityTypesEnum>
		where TUser : User<TUser, TSecurityRole, TUserPasswordHistory, TEntityTypesEnum>
		where TSecurityRole : SecurityRole<TUser, TSecurityRole, TUserPasswordHistory, TEntityTypesEnum>
		where TUserPasswordHistory : UserPasswordHistory<TUser, TSecurityRole, TUserPasswordHistory, TEntityTypesEnum>
		where TEntityTypesEnum : struct
	{
        public User()
        {
            _isValidErrorKeys = new List<string>();
			SecurityRoles = new List<TSecurityRole>();
        }
        /// <summary>
        /// Gets or sets the hub user id.
        /// </summary>
        /// <value>The hub user id.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>The username.</value>
        public string Username { get; set; }

		private string _password;
		/// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Password
        {
            get
            {
                return IsNewUser
                    ? PasswordService.HashPassword(PasswordService.GetDefaultPassword(), PasswordService.GetDefaultSalt())
                    : _password;
            }
            set { _password = value; }
        }

		private int _passwordSalt;
		/// <summary>
        /// Gets or sets the password salt.
        /// </summary>
        /// <value>The password salt.</value>
        public int PasswordSalt
        {
            get
            {
                return IsNewUser
                    ? PasswordService.GetDefaultSalt()
                    : _passwordSalt;
            }
            set { _passwordSalt = value; }
        }


		private bool _passwordChangeRequired;
		/// <summary>
        /// Gets or sets a value indicating whether this instance password change.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance password change; otherwise, <c>false</c>.
        /// </value>
        public bool PasswordChangeRequired
        {
            get
            {
                return IsNewUser
                    ? PasswordService.GetIsRequiredPasswordChange()
                    : _passwordChangeRequired;
            }
            set { _passwordChangeRequired = value; }
        }

		private DateTime _passwordChangedDate;
		/// <summary>
        /// Gets or sets the password changed date.
        /// </summary>
        /// <value>The password changed date.</value>
        public DateTime PasswordChangedDate
        {
            get
            {
                return IsNewUser
                    ? DateTime.UtcNow
                    : _passwordChangedDate;
            }
            set { _passwordChangedDate = value; }
        }

		private int _failedPasswordAttempts;
		/// <summary>
        /// Gets or sets the failed password attempts.
        /// </summary>
        /// <value>The failed password attempts.</value>
		public int FailedPasswordAttempts
        {
            get
            {
                return IsNewUser
                    ? 0
                    : _failedPasswordAttempts;
            }
            set { _failedPasswordAttempts = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is single sign on.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is single sign on; otherwise, <c>false</c>.
        /// </value>
        public bool IsSingleSignOn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance allow direct login.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance allow direct login; otherwise, <c>false</c>.
        /// </value>
        public bool AllowDirectLogin { get; set; }

		private bool _isLockedOut;
        /// <summary>
        /// Gets or sets a value indicating whether this instance is locked out.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is locked out; otherwise, <c>false</c>.
        /// </value>
        public bool IsLockedOut
        {
            get { return _isLockedOut; }
            set
            {
                if (value == false && _isLockedOut)
                {
                    FailedPasswordAttempts = 0;
                }
                _isLockedOut = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is disabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is disabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is deleted; otherwise, <c>false</c>.
        /// </value>
        public bool IsDeleted { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether this instance is a new user.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is a new user; otherwise, <c>false</c>.
        /// </value>
        public bool IsNewUser { get; set; }

        /// <summary>
        ///   Gets or sets the security roles.
        /// </summary>
        /// <value>The security roles.</value>
		public ICollection<TSecurityRole> SecurityRoles { get; set; }

        /// <summary>
        /// Gets or sets the user password history.
        /// </summary>
        /// <value>The user password history.</value>
		public ICollection<TUserPasswordHistory> UserPasswordHistories { get; set; }

		private readonly ICollection<string> _isValidErrorKeys;
		/// <summary>
        /// Gets the is valid error keys.
        /// </summary>
        public IEnumerable<string> IsValidErrorKeys
        {
            get
            {
                //IsSelfValid();
                return _isValidErrorKeys;
            }
        }

		/// <summary>
		/// Updates the password, if successful confirmation of existing password and new password different to existing password.
		/// </summary>
		/// <param name="oldPassword">The old password (un-hashed).</param>
		/// <param name="newPassword">The new password (un-hashed).</param>
		/// <param name="userPasswordHistories">The user password histories.</param>
		/// <returns>
		/// Successful update - True/False
		/// </returns>
		public bool UpdatePassword(string oldPassword, string newPassword, IEnumerable<TUserPasswordHistory> userPasswordHistories)
        {
            if (PasswordService.HashPassword(oldPassword, PasswordSalt) != Password)
            {
                _isValidErrorKeys.Add("Errors.User.ChangePasswordOldPasswordIncorrect");
                return false; //old password wasn't correct
            }

            if (oldPassword == newPassword)
            {
                _isValidErrorKeys.Add("Errors.User.ChangePasswordNewPasswordSameAsOldPassword");
                return false; //new password was same as old one
            }

            // check password strength being above organisation setting
            // TODO MN: Use setting instead of hardcoded 3
            if (PasswordService.GetPasswordStrength(newPassword) < 3)
            {
                _isValidErrorKeys.Add("Errors.User.ChangePasswordInadequatePasswordStrength");
                return false;
            }

            //Check password not used in password history.
            foreach (var userPasswordHistory in userPasswordHistories)
            {
                if (PasswordService.HashPassword(newPassword, userPasswordHistory.PasswordSalt) == userPasswordHistory.Password)
                {
                    _isValidErrorKeys.Add("Errors.User.ChangePasswordCannotReusePreviousPassword");
                    return false;
                }
            }

            //Change the password for new one and change the salt value
            PasswordSalt = PasswordService.GeneratePasswordSalt();
            Password = PasswordService.HashPassword(newPassword, PasswordSalt);
            PasswordChangedDate = DateTime.UtcNow;
            PasswordChangeRequired = false;

            return true;
        }

        #region IAuditable Members

        /// <summary>
        /// Returns a string containing a delimited list of property values in the current user object.
        /// </summary>
        public string GetAuditString()
        {
            var sb = new StringBuilder();

            sb.AppendFormat("Id:{0}|", Id);
            sb.AppendFormat("Username:{0}|", Username);
            sb.AppendFormat("Password:{0}|", Password);
            sb.AppendFormat("PasswordSalt:{0}|", PasswordSalt);
            sb.AppendFormat("PasswordChange:{0}|", PasswordChangeRequired);
            sb.AppendFormat("PasswordChangedDate:{0}|", PasswordChangedDate);
            sb.AppendFormat("FailedPasswordAttempts:{0}|", FailedPasswordAttempts);
            sb.AppendFormat("IsSingleSignOn:{0}|", IsSingleSignOn);
            sb.AppendFormat("AllowDirectLogin:{0}|", AllowDirectLogin);
            sb.AppendFormat("IsLockedOut:{0}|", IsLockedOut);
            sb.AppendFormat("IsDisabled:{0}|", IsDisabled);
            sb.AppendFormat("IsDeleted:{0}", IsDeleted);
        	sb.AppendFormat(AuditStringSuffix);
            return sb.ToString();
        }

		/// <summary>
		/// Returns an entity type value for auditing purposes.
		/// </summary>
		public abstract TEntityTypesEnum GetEntityType();

		#endregion

		/// <summary>
		/// Gets a string to be appended to the standard Audit information.
		/// </summary>
		protected virtual string AuditStringSuffix {
			get {
				return string.Empty;
			}
		}
	}
}
