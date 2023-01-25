
using Domain.Base;
using Domain.Entities;
using System.Collections.Generic;

namespace Domain.Aplicacion
{
    public class EntidadBuilder<T, TP> : BuilderBase<T, TP>
        where T : EntidadBuilder<T, TP>
        where TP : Entidad, new()
    {
        public T SetContactos(List<EntidadContacto> contacto)
        {
            Obj.Contacto = contacto;
            return This;
        }
        public T SetNit(string nit)
        {
            Obj.Nit= nit;
            return This;
        }
        public T SetRazonSocial(string razonSocial)
        {
            Obj.RazonSocial = razonSocial;
            return This;
        }
        public T SetMunicipio(Municipio municipio)
        {
            Obj.Municipio= municipio;
            return This;
        }
        public T SetCorregimiento(Corregimiento corregimiento)
        {
            Obj.Corregimiento = corregimiento;
            return This;
        }
        public T SetAutorizaNotificacionPorEmail(bool autorizacion){
            Obj.AutorizaNotificacionPorEmail = autorizacion;
            return This;
        }
    }    
}
