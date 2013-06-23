namespace MyLibrary.Web.Security
{
	using sys = System.Web.Security;

	public class MembershipProvider : sys.MembershipProvider {

		public override string ApplicationName
		{
			get
			{
				throw new System.NotImplementedException();
			}
			set
			{
				throw new System.NotImplementedException();
			}
		}

		public override bool ChangePassword(string username, string oldPassword, string newPassword)
		{
			throw new System.NotImplementedException();
		}

		public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
		{
			throw new System.NotImplementedException();
		}

		public override sys.MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out sys.MembershipCreateStatus status)
		{
			throw new System.NotImplementedException();
		}

		public override bool DeleteUser(string username, bool deleteAllRelatedData)
		{
			throw new System.NotImplementedException();
		}

		public override bool EnablePasswordReset
		{
			get { throw new System.NotImplementedException(); }
		}

		public override bool EnablePasswordRetrieval
		{
			get { throw new System.NotImplementedException(); }
		}

		public override sys.MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			throw new System.NotImplementedException();
		}

		public override sys.MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			throw new System.NotImplementedException();
		}

		public override sys.MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
		{
			throw new System.NotImplementedException();
		}

		public override int GetNumberOfUsersOnline()
		{
			throw new System.NotImplementedException();
		}

		public override string GetPassword(string username, string answer)
		{
			throw new System.NotImplementedException();
		}

		public override sys.MembershipUser GetUser(string username, bool userIsOnline)
		{
			throw new System.NotImplementedException();
		}

		public override sys.MembershipUser GetUser(object providerUserKey, bool userIsOnline)
		{
			throw new System.NotImplementedException();
		}

		public override string GetUserNameByEmail(string email)
		{
			throw new System.NotImplementedException();
		}

		public override int MaxInvalidPasswordAttempts
		{
			get { throw new System.NotImplementedException(); }
		}

		public override int MinRequiredNonAlphanumericCharacters
		{
			get { throw new System.NotImplementedException(); }
		}

		public override int MinRequiredPasswordLength
		{
			get { throw new System.NotImplementedException(); }
		}

		public override int PasswordAttemptWindow
		{
			get { throw new System.NotImplementedException(); }
		}

		public override sys.MembershipPasswordFormat PasswordFormat
		{
			get { throw new System.NotImplementedException(); }
		}

		public override string PasswordStrengthRegularExpression
		{
			get { throw new System.NotImplementedException(); }
		}

		public override bool RequiresQuestionAndAnswer
		{
			get { throw new System.NotImplementedException(); }
		}

		public override bool RequiresUniqueEmail
		{
			get { throw new System.NotImplementedException(); }
		}

		public override string ResetPassword(string username, string answer)
		{
			throw new System.NotImplementedException();
		}

		public override bool UnlockUser(string userName)
		{
			throw new System.NotImplementedException();
		}

		public override void UpdateUser(sys.MembershipUser user)
		{
			throw new System.NotImplementedException();
		}

		public override bool ValidateUser(string username, string password)
		{
			throw new System.NotImplementedException();
		}
	}
}
