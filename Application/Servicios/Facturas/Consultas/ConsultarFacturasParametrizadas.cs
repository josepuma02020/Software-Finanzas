using Domain.Contracts;
using Domain.Repositories;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.Facturas.Consultas
{
    public class ConsultarFacturasParametrizadasQuery
    {
        private readonly IUnitOfWork _unitOfWork;
        public ConsultarFacturaParametrizadaValidator Validator { get; }
        public ConsultarFacturasParametrizadasQuery(IUnitOfWork unitOfWork,IValidator<ConsultarFacturasParametrizadoRequest> validator)
        {
            _unitOfWork = unitOfWork;
            Validator = validator as ConsultarFacturaParametrizadaValidator;
        }

        public Task<ConsultarFacturasParametrizadoResponse> Handle(ConsultarFacturasParametrizadoRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(
                new ConsultarFacturasParametrizadoResponse(
                    _unitOfWork.FacturaRepository.GetFacturasParametrizado(request.Request)));
        }
    }
    public class ConsultarFacturasParametrizadoRequest : IRequest<ConsultarFacturasParametrizadoResponse>
    {
        public GetFacturasParametrizadaRequest Request { get; set; }
    }
    public class ConsultarFacturasParametrizadoResponse
    {
        public ConsultarFacturasParametrizadoResponse(IEnumerable<ConsultaFacturasDto> Facturas)
        {
            Facturas = Facturas;
            Mensaje = $"Se obtuvieron {Facturas.Count()} con el filtro especificado";
        }

        public string Mensaje { get; }
        public IEnumerable<ConsultaFacturasDto> Facturas { get; }
    }
    public class ConsultarFacturaParametrizadaValidator : AbstractValidator<ConsultarFacturasParametrizadoRequest>
    {
        private readonly IUnitOfWork _unitOfWork;
        public ConsultarFacturaParametrizadaValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }
        private void SetUpValidators()
        {

            RuleFor(e => e.Request).NotEmpty().WithMessage($"El request con los parametros de filtracion es obligatorio.");

        }
    }
}
