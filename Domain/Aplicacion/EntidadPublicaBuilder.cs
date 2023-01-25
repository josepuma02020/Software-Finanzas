
using Domain.Entities;

namespace Domain.Aplicacion
{
    public class EntidadPublicaBuilder<T, TP> : EntidadBuilder<T, TP>
        where T : EntidadBuilder<T, TP>
        where TP : EntidadPublica, new()
    {
    }
    public class EntidadPublicaBuilder : EntidadPublicaBuilder<EntidadPublicaBuilder, EntidadPublica> { }
}
