namespace MyLibrary.Data
{
    public interface IAuditable<TEntityTypeEnum>
		where TEntityTypeEnum : struct
    {
        int Id { get; }
		TEntityTypeEnum GetEntityType();
        string GetAuditString();
    }
}
