namespace MyLibrary.Security
{
	using System;

	/// <summary>
	/// A class representing a user password history object.
	/// </summary>
	public class UserPasswordHistory<TUser, TSecurityRole, TUserPasswordHistory, TEntityTypesEnum> 
		where TUser : User<TUser, TSecurityRole, TUserPasswordHistory , TEntityTypesEnum>
		where TSecurityRole : SecurityRole<TUser, TSecurityRole, TUserPasswordHistory, TEntityTypesEnum>
		where TUserPasswordHistory : UserPasswordHistory<TUser, TSecurityRole, TUserPasswordHistory, TEntityTypesEnum>
		where TEntityTypesEnum : struct
	{
		/// <summary>
		/// Gets or sets the user password history id.
		/// </summary>
		/// <value>The user password history id.</value>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the user id.
		/// </summary>
		/// <value>The user id.</value>
		public int UserId { get; set; }

		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		/// <value>The password.</value>
		public string Password { get; set; }

		/// <summary>
		/// Gets or sets the password salt.
		/// </summary>
		/// <value>The password salt.</value>
		public int PasswordSalt { get; set; }

		/// <summary>
		/// Gets or sets the password set date.
		/// </summary>
		/// <value>The password set date.</value>
		public DateTime PasswordSetDate { get; set; }

		/// <summary>
		/// Gets or sets the hub user.
		/// </summary>
		/// <value>The hub user.</value>
		public TUser User { get; set; }
	}
}
