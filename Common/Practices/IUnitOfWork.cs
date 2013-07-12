namespace Avantech.Common.Practices
{
    /// <summary>
    /// Interface to standardise basic functionality required by all Units Of Work
    /// These typically wrap all contexts used by repositories, providing functionality to save changes across all repositories
    /// once all work on the repositories has been completed
    /// Units of Work are utilised by repository consumers.
    /// </summary>
    public interface IUnitOfWork
    {
        void SaveChanges();
    }
}
