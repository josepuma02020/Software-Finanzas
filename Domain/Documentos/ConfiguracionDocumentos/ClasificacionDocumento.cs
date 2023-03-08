using Domain.Aplicacion.EntidadesConfiguracion;
using Domain.Base;
using Domain.Entities;
using Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Documentos.ConfiguracionDocumentos
{
    public class ClasificacionDocumento : Entity<Guid>
    {
        public string Descripcion { get; set; }
        public ProcesosDocumentos ClasificacionProceso { get; set; }
        public string NombreClasificacion => ClasificacionProceso.GetDescription();
        private ClasificacionDocumento() : base(null)
        {
        }
        public ClasificacionDocumento(Usuario? usuariocreador) : base(usuariocreador)
        {
        }
        #region Configuracion Infraestructure
        public IEnumerable<NotaContable> NotasContables { get; set; }
        #endregion

    }
}
