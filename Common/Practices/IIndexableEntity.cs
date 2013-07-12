    namespace Avantech.Common.Practices
{
    public interface IIndexableEntity<TId> : IEntity<TId>
    {
        string IndexingName { get; }
    }
}
