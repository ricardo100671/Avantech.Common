namespace MyLibrary.Data
{
	using System;
	using System.Data;
	using System.Data.Objects;

	public interface IUnitOfWork : IDisposable
    {
        bool IsInTransaction { get; }

        void SaveChanges();

        void SaveChanges(SaveOptions saveOptions);

        void BeginTransaction();

        void BeginTransaction(IsolationLevel isolationLevel);

        void RollBackTransaction();

        void CommitTransaction();
    }
}
