using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Domain.Base;
using Domain.Entities;
using Domain.Extensions;

namespace Domain.Clases
{
    public class NotaContable : Entity<Guid>
    {
        public string? Fechabatch { get; set; }
        public string? Batch { get; set; }
        public string Comentario { get; set; }
        public int Importe { get; set; }
        public ClasificacionDocumento ClasificacionDocumento { get; set; }
        public string DescripcionClasificacionDocumento =>ClasificacionDocumento.Descripcion;
        public virtual EstadoNotaContable EstadoNotaContable { get;  set; }
        public string DescripcionEstadoNotaContable => EstadoNotaContable.GetDescription();
        public List<Registrodenotacontable>  Registrosnota { get; set; }
        public virtual Tiponotacontable Tiponotacontable { get;  set; }
        public string TipoEntidadNombre => Tiponotacontable == Tiponotacontable.Soportes ? "Soportes" : "registrosnota";
        public NotaContable()
        {
            EstadoNotaContable = EstadoNotaContable.Abierto;
            Batch = null;
        }
    }
    public enum Tiponotacontable
    {
        Soportes,
        registrosnota,
    }
    public enum EstadoNotaContable
    {
        [Description("Abierto")] Abierto,
        [Description("Revision")] Revision,
        [Description("Aprobado")] Aprobado,
        [Description("Autorizado")] Autorizado,
        [Description("Cerrado")] Cerrado,
    }
    public class Registrodenotacontable:Entity<Guid>
    {
        public string ? Haber { get; set; }
        public string ? Debe { get; set; }
        public string ? Lm { get; set; }
        public string ? Tipolm{ get; set; }
        public Tercero Tercero { get; set; }
        public Cuenta cuenta { get; set; }
        public string NotaContableId { get; set; }

 
    }   
}
