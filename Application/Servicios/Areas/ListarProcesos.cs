using Domain.Aplicacion;
using Domain.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.Areas
{
    class ListarProcesosQuery : IRequestHandler<ListarProcesosQueryRequest, ListarProcesosQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ListarProcesosQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<ListarProcesosQueryResponse> Handle(ListarProcesosQueryRequest request, CancellationToken cancellationToken)
        {
            var procesos = _unitOfWork.GenericRepository<Proceso>().GetAll();
            return Task.FromResult(new ListarProcesosQueryResponse(procesos));
        }
    }
    public class ListarProcesosQueryRequest : IRequest<ListarProcesosQueryResponse>
    {
    }
    public class ListarProcesosQueryResponse
    {
        public ListarProcesosQueryResponse(IEnumerable<Proceso> procesos)
        {
            Procesos = procesos;
        }
        public ListarProcesosQueryResponse()
        {

        }

        public IEnumerable<Proceso> Procesos { get; }
    }
}
