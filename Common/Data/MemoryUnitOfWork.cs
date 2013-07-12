
using System;

namespace Avantech.Common.Data {
    public class MemoryUnitOfWork : IUnitOfWork {
        
        public bool IsInTransaction {
            get { throw new NotImplementedException(); }
        }

        public void SaveChanges() {
            throw new NotImplementedException();
        }

        public void SaveChanges(System.Data.Objects.SaveOptions saveOptions) {
            throw new NotImplementedException();
        }

        public void BeginTransaction() {
            throw new NotImplementedException();
        }

        public void BeginTransaction(System.Data.IsolationLevel isolationLevel) {
            throw new NotImplementedException();
        }

        public void RollBackTransaction() {
            throw new NotImplementedException();
        }

        public void CommitTransaction() {
            throw new NotImplementedException();
        }

        public void Dispose() {
            throw new NotImplementedException();
        }
    }
}
