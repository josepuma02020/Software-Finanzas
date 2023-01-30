using Domain.Aplicacion;
using Domain.Base;
using Domain.Clases;
using Domain.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public enum Rol
    {
        [Description("Normal")] Normal,
        [Description("Verificador de notascontables")] Verificadordenotascontables,
        [Description("Aprobador de notascontables")] Aprobadordenotascontables,
        [Description("Autorizador de notascontables")] Autorizadordenotascontables,
        [Description("Aprobador de facturas")] Aprobadordefacturas,
        [Description("Administrador")] Administrador,
        [Description("Administrador de notas contables")] AdminitradorNotaContable,
        [Description("Administrador de facturas")] AdministradorFactura,
    }



}
