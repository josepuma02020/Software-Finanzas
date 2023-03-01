using Domain.Aplicacion.EntidadesConfiguracion;
using Domain.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.Aplicacion.Areas.Equipos
{
    class ListarProcesosporEquipoQuery : IRequestHandler<ListarProcesosporEquipoQueryRequest, ListarProcesosporEquipoQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ListarProcesosporEquipoQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task<ListarProcesosporEquipoQueryResponse> Handle(ListarProcesosporEquipoQueryRequest request, CancellationToken cancellationToken)
        {
            var procesos = _unitOfWork.GenericRepository<Proceso>().FindBy(e => e.EquipoId == request.equipo.Id);
            return Task.FromResult(new ListarProcesosporEquipoQueryResponse(procesos));
        }
    }
    public class ListarProcesosporEquipoQueryRequest : IRequest<ListarProcesosporEquipoQueryResponse>
    {
        public Equipo equipo;
    }
    public class ListarProcesosporEquipoQueryResponse
    {
        public ListarProcesosporEquipoQueryResponse(IEnumerable<Proceso> procesos)
        {
            Procesos = procesos;
        }
        public ListarProcesosporEquipoQueryResponse()
        {

        }

        public IEnumerable<Proceso> Procesos { get; }
    }
}
