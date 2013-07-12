

namespace Avantech.Common.Practices
{
    public interface IEntity
    {
        int Id { get; set; }
    }

    public interface IEntity<TId>
    {
        TId Id { get; set; }
    }
}
