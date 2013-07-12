namespace Avantech.Common.Practices
{
    public interface IAuditableEntity<out TEntityTypeEnum> : IEnity
		where TEntityTypeEnum : struct
    {
        int Id { get; }

        TEntityTypeEnum GetEntityType();

        string GetAuditString();
    }
}
