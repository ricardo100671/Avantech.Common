namespace MyLibrary.Data.Specification
{
	using System.Linq;
	using Linq.Expressions;

	public class AndSpecification<TEntity> : CompositeSpecification<TEntity>
	{
		public AndSpecification(Specification<TEntity> leftSide, Specification<TEntity> rightSide)
			: base(leftSide, rightSide)
		{
		}

		public override TEntity SatisfyingEntityFrom(IQueryable<TEntity> query)
		{
			return SatisfyingEntitiesFrom(query).FirstOrDefault();
		}

		public override IQueryable<TEntity> SatisfyingEntitiesFrom(IQueryable<TEntity> query)
		{
			return query.Where(_leftSide.Predicate.And(_rightSide.Predicate));
		}
	}
}