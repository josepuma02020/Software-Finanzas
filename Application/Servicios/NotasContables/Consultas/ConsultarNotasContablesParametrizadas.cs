
using Domain.Contracts;
using Domain.Repositories;
using MediatR;

namespace Application.Servicios.NotasContables.Consultas
{
    public class ConsultarNotasContablesParametrizadasQuery : IRequestHandler<ConsultarNotasContablesParametrizadoRequest, ConsultarNotasContablesParametrizadoResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ConsultarNotasContablesParametrizadasQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<ConsultarNotasContablesParametrizadoResponse> Handle(ConsultarNotasContablesParametrizadoRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(
                new ConsultarNotasContablesParametrizadoResponse(
                    _unitOfWork.NotaContableRepository.GetNotasContablesParametrizadas(request.Request)));
        }
    }
    public class ConsultarNotasContablesParametrizadoRequest : IRequest<ConsultarNotasContablesParametrizadoResponse>
    {
        public GetNotasContablesParametrizadaRequest Request { get; set; }
    }
    public class ConsultarNotasContablesParametrizadoResponse
    {
        public ConsultarNotasContablesParametrizadoResponse(IEnumerable<ConsultaNotasContablesDTO> notascontables)
        {
            NotasContables = notascontables;
            Mensaje = $"Se obtuvieron {notascontables.Count()} con el filtro especificado";
        }

        public string Mensaje { get; }
        public IEnumerable<ConsultaNotasContablesDTO> NotasContables { get; }
    }
}
