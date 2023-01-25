
using System;
using Domain.Base;

namespace Domain.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> GenericRepository<T>() where T : BaseEntity;

        int Commit();
    }
}
