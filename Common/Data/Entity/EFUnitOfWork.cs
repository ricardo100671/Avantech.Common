
using System;
using System.Data;
using System.Data.Common;
using System.Data.Objects;

namespace Avantech.Common.Data.Entity
{
    internal class EFUnitOfWork : IUnitOfWork
    {
        private DbTransaction _transaction;
        private DbContext _dbContext;

        public EFUnitOfWork(DbContext context)
        {
            _dbContext = context;
        }

        public bool IsInTransaction
        {
            get { return _transaction != null; }
        }

        public void BeginTransaction()
        {
            BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            if (_transaction != null)
            {
                throw new ApplicationException("Cannot begin a new transaction while an existing transaction is still running. " +
                                                "Please commit or rollback the existing transaction before starting a new one.");
            }
            OpenConnection();
            _transaction = _dbContext.Database.Connection.BeginTransaction(isolationLevel);
        }        

        public void RollBackTransaction()
        {
            if (_transaction == null) {
                throw new ApplicationException("A Transaction has not been started.");
            }
            _transaction.Rollback();
        }

        public void CommitTransaction()
        {
            if (_transaction == null)
            {
                throw new ApplicationException("A Transaction has not been started.");
            }
            try
            {
                ((IObjectContextAdapter)_dbContext).ObjectContext.SaveChanges();
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
            finally
            {
                ReleaseCurrentTransaction();
            }
        }

        public void SaveChanges()
        {
            if (IsInTransaction)
            {
                throw new ApplicationException("A transaction is currently open. Use RollBackTransaction or CommitTransaction instead.");
            }
            try {
                _dbContext.SaveChanges();
            }
            catch(DbUpdateConcurrencyException ex){
                throw new Exception(ex.Message);
            }
        }

        public void SaveChanges(SaveOptions saveOptions)
        {
            if (IsInTransaction)
            {
                throw new ApplicationException("A transaction is currently open. Use RollBackTransaction or CommitTransaction instead.");
            }
            ((IObjectContextAdapter)_dbContext).ObjectContext.SaveChanges(saveOptions);
        }

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes off the managed and unmanaged resources used.
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            if (_disposed)
                return;

            _disposed = true;
        }

        private bool _disposed;
        #endregion

        private void OpenConnection()
        {
            if (((IObjectContextAdapter)_dbContext).ObjectContext.Connection.State != ConnectionState.Open)
            {
                ((IObjectContextAdapter)_dbContext).ObjectContext.Connection.Open();
            }
        }

        /// <summary>
        /// Releases the current transaction
        /// </summary>
        private void ReleaseCurrentTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }
    }
}
