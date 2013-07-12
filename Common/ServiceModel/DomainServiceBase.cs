using System;
using System.Collections.Generic;
using System.Linq;

namespace Avantech.Common.ServiceModel
{
    using Avantech.Common.Data;

    public class DomainServiceBase<TIRepository, TEntity, TId>
        where TIRepository : IObsoleteRepository<TEntity, TId>
        where TEntity : class, IEntity<TId>
        where TId : struct, IEquatable<TId>
    {
        public DomainServiceBase(
            TIRepository repository,
            params IUnitOfWork[] unitsOfWork
        ) {
            if (repository == null) throw new ArgumentNullException("repository");
            if (unitsOfWork == null) throw new ArgumentNullException("unitsOfWork");

            _repository = repository;
            _unitsOfWork = unitsOfWork.ToList();
        }

        readonly TIRepository _repository;
        protected TIRepository Repository
        {
            get { return _repository; }
        }

        readonly List<IUnitOfWork> _unitsOfWork;
        protected List<IUnitOfWork> UnitsOfWork
        {
            get { return _unitsOfWork; }
        }
    }
}
