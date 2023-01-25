using Domain.Contracts;
using Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.Facturas
{
    public class RegistrarFactura : IRequestHandler<RegistrarFacturaDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegistrarFacturaDtoValidator Validator { get; }

        public RegistrarFactura(IUnitOfWork unitOfWork, IValidator<RegistrarFacturaDto> validator)
        {
            _unitOfWork = unitOfWork;
            Validator = validator as RegistrarFacturaDtoValidator;
        }
        public Task<Response> Handle(RegistrarFacturaDto request, CancellationToken cancellationToken)
        {
            var nuevafactura = new Factura()
            {
                 CodigoTercero= request.CodigoTercero,
                  fechapago=request.fechapago,
                  UsuarioQueRegistroFactura=request.UsuarioQueRegistroFactura,
                  Valor=request.Valor,
                  Observaciones=request.Observaciones,
                  ri=request.ri,
                Id = Guid.NewGuid(),
            };


            _unitOfWork.GenericRepository<Factura>().Add(nuevafactura);
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Data = nuevafactura,
                Mensaje = $"La factura se registró correctamente."
            });
        }
    }
    public class RegistrarFacturaDto : IRequest<Response>
    {
        public Usuario UsuarioQueRegistroFactura { get; set; }
        public Tercero? CodigoTercero { get; set; }
        public int Valor { get; set; }
        public string Observaciones { get; set; }

        public string fechapago { get; set; }
        public string ri { get; set; }
    }
    public class RegistrarFacturaDtoValidator : AbstractValidator<RegistrarFacturaDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public RegistrarFacturaDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(e => e.Valor).NotEmpty().GreaterThanOrEqualTo(1000).WithMessage("El valor de la factura debe ser mayor a 1000.");
            RuleFor(e => e.fechapago).NotEmpty().Length(5, 50);

        }

    }
}
