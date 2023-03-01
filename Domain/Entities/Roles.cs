using Domain.Aplicacion;
using Domain.Base;
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
        [Description("Verificador de facturas")] VerificadorFacturas,
        [Description("Administrador")] Administrador,
        [Description("Administrador de notas contables")] AdministradorNotaContable,
        [Description("Administrador de facturas")] AdministradorFactura,
        [Description("Administrador de flujo de caja")] AdministradorFlujodeCaja,
        [Description("Robot Uipath")] Bot,
    }



}
