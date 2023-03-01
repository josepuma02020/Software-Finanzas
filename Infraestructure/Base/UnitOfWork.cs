using Domain.Base;
using Domain.Contracts;
using Domain.Repositories;
using Infraestructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Base
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private IDbContext _dbContext;

        public INotaContableRepository _notacontablerepository;

        public IFacturaRepository _facturarepository;

        public IUsuarioRepository _usuarioRepository;
        public UnitOfWork(IDbContext context)
        {
            _dbContext = context;
        }

        public IGenericRepository<T> GenericRepository<T>() where T : BaseEntity
        {
            return new GenericRepository<T>(_dbContext);
        }
        public INotaContableRepository NotaContableRepository
        {
            get { return _notacontablerepository ?? (_notacontablerepository = new NotaContableRepository(_dbContext)); }
        }
        public IFacturaRepository FacturaRepository
        {
            get { return _facturarepository ?? (_facturarepository = new FacturaRepository(_dbContext)); }
        }
        public IUsuarioRepository UsuarioRepository
        {
            get { return _usuarioRepository ?? (_usuarioRepository = new UsuarioRepository(_dbContext)); }
        }
        public int Commit()
        {
            return _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Disposes all external resources.
        /// </summary>
        /// <param name="disposing">The dispose indicator.</param>
        private void Dispose(bool disposing)
        {
            if (disposing && _dbContext != null)
            {
                ((DbContext)_dbContext).Dispose();
                _dbContext = null;
            }
        }
    }
}
