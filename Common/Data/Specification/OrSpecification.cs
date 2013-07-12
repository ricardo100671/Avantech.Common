using Avantech.Common.Linq.Expressions;

using System.Linq;

namespace Avantech.Common.Data.Specification
{
    public class OrSpecification<TEntity> : CompositeSpecification<TEntity>
	{
		public OrSpecification(Specification<TEntity> leftSide, Specification<TEntity> rightSide)
			: base(leftSide, rightSide){
		}

		public override TEntity SatisfyingEntityFrom(IQueryable<TEntity> query)
		{
			return SatisfyingEntitiesFrom(query).FirstOrDefault();
		}

		public override IQueryable<TEntity> SatisfyingEntitiesFrom(IQueryable<TEntity> query)
		{
			return query.Where(_leftSide.Predicate.Or(_rightSide.Predicate));
		}
	}
}