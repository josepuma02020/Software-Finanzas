using Domain.Clases;
using Domain.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.NotasContables
{
    class ListarRegistrosNotaContableQuery : IRequestHandler<ListarRegistrosNotaContableQueryRequest, ListarRegistrosNotaContableQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ListarRegistrosNotaContableQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<ListarRegistrosNotaContableQueryResponse> Handle(ListarRegistrosNotaContableQueryRequest request, CancellationToken cancellationToken)
        {
            var registros = _unitOfWork.GenericRepository<Registrodenotacontable>().FindBy(e => e.NotaContableId == request.notacontable.Id);
            return Task.FromResult(new ListarRegistrosNotaContableQueryResponse(registros));
        }
    }
    public class ListarRegistrosNotaContableQueryRequest : IRequest<ListarRegistrosNotaContableQueryResponse>
    {
        public NotaContable notacontable;
    }
    public class ListarRegistrosNotaContableQueryResponse
    {
        public ListarRegistrosNotaContableQueryResponse(IEnumerable<Registrodenotacontable> registros)
        {
            Registros = registros;
        }
        public ListarRegistrosNotaContableQueryResponse()
        {

        }

        public IEnumerable<Registrodenotacontable> Registros { get; }
    }
}
