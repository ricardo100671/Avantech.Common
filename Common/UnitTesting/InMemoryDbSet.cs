using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace Avantech.Common.UnitTesting
{
    /// <summary>
    /// Provides a mockable DbSet that can be used to test repositories created with Entity Framework
    /// Similar to InMemoryDbSet available from NuGet but all methods are virtual
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InMemoryDbSet<T> : IDbSet<T> where T : class
    {
        protected static readonly HashSet<T> _Data = new HashSet<T>();
        private readonly IQueryable _Query = _Data.AsQueryable();

        public InMemoryDbSet(params T[] entities)
        {
            Clear();
            entities
                .ToList()
                .ForEach(e =>
                    _Data.Add(e)
                );
        }

        public virtual void AddRange(params T[] entities)
        {
            entities
                .ToList()
                .ForEach(e =>
                    Add(e)
                );
        }

        public ObservableCollection<T> Local
        {
            get
            {
                return new ObservableCollection<T>(_Data);
            }
        }

        public Type ElementType
        {
            get
            {
                return _Query.ElementType;
            }
        }

        public Expression Expression
        {
            get
            {
                return _Query.Expression;
            }
        }

        public IQueryProvider Provider
        {
            get
            {
                return _Query.Provider;
            }
        }

        static InMemoryDbSet()
        {
        }

        public InMemoryDbSet()
            : this(false)
        {
        }

        public InMemoryDbSet(bool clearDownExistingData)
        {
            if (!clearDownExistingData)
                return;
            Clear();
        }

        public virtual void Clear()
        {
            _Data.Clear();
        }

        public virtual T Add(T entity)
        {
            _Data.Add(entity);
            return entity;
        }

        public virtual T Attach(T entity)
        {
            _Data.Add(entity);
            return entity;
        }

        public virtual TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, T
        {
            throw new NotImplementedException();
        }

        public virtual T Create()
        {
            return Activator.CreateInstance<T>();
        }

        public virtual T Find(params object[] keyValues)
        {
            throw new NotImplementedException("Derive from InMemoryDbSet and override Find.");
        }

        public virtual T Remove(T entity)
        {
            _Data.Remove(entity);
            return entity;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _Data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _Data.GetEnumerator();
        }
    }
}
