using System.Web.Security;

namespace Avantech.Common.Web.Security
{
    public class MembershipProxy : IMembershipProxy
    {
        public string GeneratePassword(int length, int numberOfNonAlphanumericCharacters)
        {
            return Membership.GeneratePassword(length, numberOfNonAlphanumericCharacters);
        }
    }
}
