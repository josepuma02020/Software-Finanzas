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
    public class NotaContable : BaseEntityDocumento
    {
        public DateTime? Fechabatch { get; set; }
        public string? Batch { get; set; }
        public Usuario UsuarioCreador { get; set; }
        public string Comentario { get; set; }
        public int Importe { get; set; }
        public List<AppFile> soportes { get; set; }
        public ClasificacionDocumento ClasificacionDocumento { get; set; }
        public string DescripcionClasificacionDocumento =>ClasificacionDocumento.Descripcion;
        public List<Registrodenotacontable>  Registrosnota { get; set; }
        public ProcesosDocumentos ProcesoDocumento { get; set; }
        public virtual Tiponotacontable Tiponotacontable { get;  set; }
        public virtual TipoDocumento TipoDocumento { get; set; }
        public string TipoEntidadNombre => Tiponotacontable == Tiponotacontable.Soportes ? "Soportes" : "registrosnota";
        public NotaContable SetRegistrosNotaContable(List<Registrodenotacontable> registrosnota)
        {
            this.Registrosnota = registrosnota;
            return this;
        }
    }
    public enum Tiponotacontable
    {
        Soportes,
        registrosnota,
    }
    public class Registrodenotacontable:Entity<Guid>
    {
        public DateTime? Fecha { get; set; }
        public string ? Haber { get; set; }
        public string ? Debe { get; set; }
        public string ? Lm { get; set; }
        public string ? Tipolm{ get; set; }
        public Tercero? Tercero { get; set; }
        public Cuenta? Cuenta { get; set; }
        public Guid NotaContableId { get; set; }

 
    }   
}
