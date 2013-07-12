
using Avantech.Common.Data;
using System.Collections.Generic;
using System.Text;

namespace Avantech.Common.Security {
    /// <summary>
	/// A specific role related to security.
	/// </summary>
	public abstract class SecurityRole<TUser, TSecurityRole, TUserPasswordHistory, TEntityTypesEnum> : IAuditable<TEntityTypesEnum>
		where TUser : User<TUser, TSecurityRole, TUserPasswordHistory, TEntityTypesEnum>
		where TSecurityRole : SecurityRole<TUser, TSecurityRole, TUserPasswordHistory, TEntityTypesEnum>
		where TUserPasswordHistory : UserPasswordHistory<TUser, TSecurityRole, TUserPasswordHistory, TEntityTypesEnum>
		where TEntityTypesEnum : struct
	{
		/// <summary>
		/// The unique id allocated by the database.
		/// </summary>
		/// <value>The security role id.</value>
		public int Id { get; set; }

		/// <summary>
		/// The name of the security role.
		/// </summary>
		/// <value>The role name.</value>
		public string RoleName { get; set; }

		/// <summary>
		/// A description of the scurity role.
		/// </summary>
		/// <value>The role description.</value>
		public string RoleDescription { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is deleted.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is deleted; otherwise, <c>false</c>.
		/// </value>
		public bool IsDeleted { get; set; }

		/// <summary>
		/// Gets or sets the users.
		/// </summary>
		/// <value>The users.</value>
		public ICollection<TUser> Users { get; set; }

		#region IAuditable Members

		/// <summary>
		/// Returns a string containing a delimited list of property values in the current security role object.
		/// </summary>
		public string GetAuditString() {
			var sb = new StringBuilder();

			sb.AppendFormat("Id:{0}|", Id);
			sb.AppendFormat("RoleName:{0}|", RoleName);
			sb.AppendFormat("RoleDescription:{0}|", RoleDescription);
			sb.AppendFormat("IsDeleted:{0}", IsDeleted);

			return sb.ToString();
		}

		/// <summary>
		/// Returns an entity type value for auditing purposes.
		/// </summary>
		public abstract TEntityTypesEnum GetEntityType();

		#endregion
	}
}