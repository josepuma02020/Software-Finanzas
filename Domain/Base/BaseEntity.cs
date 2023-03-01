
using Domain.Entities;
using System;

namespace Domain.Base
{
    public abstract class BaseEntity
    {
        public DateTime FechaDeCreacion { get; set; } = DateTime.Now;
        public DateTime? FechaEdicion { get; set; }
        public Usuario? UsuarioCreador { get; protected set; }
        public Usuario? UsuarioEditor { get; set; }
        public Guid? UsuarioCreadorId { get; set; }
        public Guid? UsuarioEditorId { get; set; }

        public BaseEntity(Usuario? usuariocreador)
        {
            UsuarioCreador = usuariocreador;
            UsuarioCreadorId = usuariocreador?.Id;
        }
    }    
    public abstract class Entity<T> : BaseEntity, IEntity<T> 
    {

        protected Entity(Usuario? usuariocreador) : base(usuariocreador)
        {
        }

        public virtual T Id { get ; set ; }

        public Entity<T> SetId(T id)
        {
            this.Id = id;
            return this;
        }
        
    }
}
