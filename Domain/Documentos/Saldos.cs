using Domain.Aplicacion.Entidades;
using Domain.Aplicacion.Entidades.CuentasBancarias;
using Domain.Base;
using Domain.Documentos.ConfiguracionDocumentos;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Documentos
{
    public class Saldos : BaseEntityDocumento
    {
        public CuentaBancaria CuentaBancaria { get; set; }
        public Guid CuentaBancariaId { get; set; }
        public Guid EntidadId { get; set; }
        public long SaldoTotal { get; set; }
        public long? SaldoDisponible { get; set; }
        public bool TieneDisponible { get; set; }
        private Saldos() : base(null)
        {
            ProcesoDocumento = ProcesosDocumentos.SaldosDiarios;
        }
        public Saldos(Usuario usuariocreado):base(usuariocreado)
        {
            ProcesoDocumento = ProcesosDocumentos.SaldosDiarios;
        }
        public Saldos(Usuario usuariocreado,CuentaBancaria cuentaBancaria) : base(usuariocreado)
        {
            ProcesoDocumento = ProcesosDocumentos.SaldosDiarios;
            CuentaBancaria = cuentaBancaria;
            CuentaBancariaId = cuentaBancaria.Id ;
        }

    }
}
