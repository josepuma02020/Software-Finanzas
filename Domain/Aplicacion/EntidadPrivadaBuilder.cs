
using Domain.Entities;

namespace Domain.Aplicacion
{
    public class EntidadPrivadaBuilder<T, TP> : EntidadBuilder<T, TP>
        where T : EntidadBuilder<T, TP>
        where TP : EntidadPrivada, new()
    {        
        public T SetRepresentanteLegal(string representanteLegal)
        {
            Obj.RepresentanteLegal = representanteLegal;
            return This;
        }
        public T SeTEstadoEstablecimiento(EstadoDelEstablecimiento estadoEstablecimiento)
        {
            Obj.EstadoDelEstablecimiento = estadoEstablecimiento;
            return This;
        }
        public T SetTamanioDeLaEmpresa(TamanioDeLaEmpresa tamanioDeLaEmpresa)
        {
            Obj.TamanioDeLaEmpresa = tamanioDeLaEmpresa;
            return This;
        }
        public T SetClaseDeEstablecimiento(ClaseDeEstablecimiento claseDeEstablecimiento)
        {
            Obj.ClaseDeEstablecimiento = claseDeEstablecimiento;
            return This;
        }
    }
    public class EntidadPrivadaBuilder : EntidadPrivadaBuilder<EntidadPrivadaBuilder, EntidadPrivada> { }
}
