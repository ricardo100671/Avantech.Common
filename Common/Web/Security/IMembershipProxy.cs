namespace Avantech.Common.Web.Security
{
    public interface IMembershipProxy
    {
        string GeneratePassword(int length, int numberOfNonAlphanumericCharacters);
    }
}
