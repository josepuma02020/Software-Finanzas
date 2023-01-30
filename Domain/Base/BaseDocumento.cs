using Domain.Clases;
using Domain.Entities;
using Domain.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Base
{
    public abstract class BaseEntityDocumento : Entity<Guid>
    {
        public DateTime FechaDeCreacion { get; set; } = DateTime.Now;
        public Usuario UsuarioCreador { get; set; }
        public Usuario? Aprobador { get; set; }
        public Usuario? Autorizador { get; set; }
        public Usuario? Verificador { get; set; }
        public DateTime FechaAprobacion { get; set; }
        public DateTime FechaAutorizacion { get; set; }
        public DateTime FechaVerificacion { get; set; }
        public DateTime Fechabot { get; set; }
 
        public virtual EstadoDocumento EstadoDocumento { get; set; }
        public string DescrripcionEstadoDocumento => EstadoDocumento.GetDescription();

        public BaseEntityDocumento ()
        {
            EstadoDocumento = EstadoDocumento.Abierto;
        }
        public BaseEntityDocumento SetAprobador(Usuario aprobador)
        {
            this.EstadoDocumento = EstadoDocumento.Aprobado;
            this.Aprobador = aprobador;
            this.FechaAprobacion = DateTime.Now;
            return this;
        }
        public BaseEntityDocumento SetAutorizador(Usuario autorizador)
        {
            this.EstadoDocumento = EstadoDocumento.Autorizado;
            this.Autorizador = autorizador;
            this.FechaAutorizacion = DateTime.Now;
            return this;
        }
        public BaseEntityDocumento SetVerificador(Usuario verificador)
        {
            this.EstadoDocumento = EstadoDocumento.Verificado;
            this.Verificador = verificador;
            this.FechaVerificacion = DateTime.Now;
            return this;
        }
    }
    public enum EstadoDocumento
    {
        [Description("Abierto")] Abierto,
        [Description("Revision")] Revision,
        [Description("Aprobado")] Aprobado,
        [Description("Autorizado")] Autorizado,
        [Description("Verificado")] Verificado,
        [Description("Cerrado")] Cerrado,

    }

}
