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
            var tercero = Validator.Tercero;
            var usuario = Validator.UsuarioRegistroFactura;
            var nuevafactura = new Factura()
            {
                 Tercero= tercero,
                  fechapago=request.fechaPago,
                  UsuarioCreador=usuario,
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
        public Guid UsuarioRegistroFacturaId { get; set; }
        public Guid? TerceroId { get; set; }
        public int Valor { get; set; }
        public string? Observaciones { get; set; }
        public DateTime fechaPago { get; set; }
        public string ? ri { get; set; }
        public RegistrarFacturaDto()
        {

        }
    }
    public class RegistrarFacturaDtoValidator : AbstractValidator<RegistrarFacturaDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Usuario UsuarioRegistroFactura;
        public Tercero Tercero;
        public RegistrarFacturaDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(bdu => bdu.UsuarioRegistroFacturaId).Must(ExisteUsuario).WithMessage($"El documento suministrado no" +
               $" fué localizado en el sistema.");
            RuleFor(bdu => bdu.TerceroId.Value).Must(ExisteTercero).WithMessage($"No se encontro tercero en el sistema.");
            RuleFor(e => e.Valor).NotEmpty().GreaterThanOrEqualTo(1000).WithMessage("El valor de la factura debe ser mayor a 1000.");
            RuleFor(e => e.fechaPago).NotEmpty().WithMessage("No se encontro fecha de pago");
            RuleFor(e => e.Valor).NotEmpty().GreaterThan(1000).WithMessage("El valor del pago debe ser mayor a 1000");

        }
        private bool ExisteUsuario(Guid id)
        {
            UsuarioRegistroFactura = _unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == id);
            return UsuarioRegistroFactura != null;
        }
        private bool ExisteTercero(Guid id)
        {
            Tercero = _unitOfWork.GenericRepository<Tercero>().FindFirstOrDefault(e => e.Id == id);
            return Tercero != null;
        }

    }
}
