
using System;
using Domain.Base;
using Domain.Repositories;

namespace Domain.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> GenericRepository<T>() where T : BaseEntity;
        public INotaContableRepository NotaContableRepository { get; }
        public IFacturaRepository FacturaRepository { get; }
        public IUsuarioRepository UsuarioRepository { get; }
        int Commit();
    }
}
