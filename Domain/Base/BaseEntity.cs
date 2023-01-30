
using System;

namespace Domain.Base
{
    public abstract class BaseEntity
    {
        public DateTime FechaDeCreacion { get; set; } = DateTime.Now;
    }    
    public abstract class Entity<T> : BaseEntity, IEntity<T> 
    {
        public virtual T Id { get ; set ; }

        public Entity<T> SetId(T id)
        {
            this.Id = id;
            return this;
        }
        
    }
}
