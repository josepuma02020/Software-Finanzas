using Domain.Base;
using Domain.Entities;
using System.ComponentModel;

namespace Domain.Documentos.ConfiguracionDocumentos
{
    public class TipoDocumento : Entity<Guid>
    {
        

        public string CodigoTipoDocumento { get; set; }
        public string DescripcionTipoDocumento { get; set; }
        #region Configuracion
        public IEnumerable<NotaContable> NotasContables { get; set; }
        #endregion
        private TipoDocumento() : base(null)
        {
        }
        public TipoDocumento(Usuario? usuariocreador) : base(usuariocreador)
        {
        }
    }
    public enum ProcesosDocumentos
    {
        [Description("Facturas")] Facturas,
        [Description("Notas Contable Financiacion")] NotasContable,
        [Description("Acutalizacion de saldos.")] SaldosDiarios,
    }
}
