using Domain.Base;
using Domain.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Factura : Entity<Guid>
    {
        public Usuario UsuarioQueRegistroFactura { get; set; }  
        public Usuario ? UsuarioQueVerificoFactura { get; set; }
        public Tercero ? CodigoTercero { get; set; }
        public int Valor { get; set; }
        public string  Observaciones { get; set; }
        public string fechapago { get; set; }
        public virtual EstadoFactura EstadoFactura { get;  set; }
        public string EstadoFacturaNombre => EstadoFactura == EstadoFactura.NoRevisado ? "NoRevisado" : "Revisado";
        public string  ri { get; set; }
        public Factura()
        {
            EstadoFactura = EstadoFactura.NoRevisado;
        }

    }
    public enum EstadoFactura
    {
        NoRevisado,
        Revisado,
    }
}
