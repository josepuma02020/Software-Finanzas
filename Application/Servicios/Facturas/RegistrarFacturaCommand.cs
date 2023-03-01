using Domain.Aplicacion.Entidades;
using Domain.Aplicacion.Entidades.CuentasBancarias;
using Domain.Aplicacion.EntidadesConfiguracion;
using Domain.Contracts;
using Domain.Documentos;
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
    public class RegistrarFacturaCommand : IRequestHandler<RegistrarFacturaDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegistrarFacturaDtoValidator Validator { get; }

        public RegistrarFacturaCommand(IUnitOfWork unitOfWork, IValidator<RegistrarFacturaDto> validator)
        {
            _unitOfWork = unitOfWork;
            Validator = validator as RegistrarFacturaDtoValidator;
        }
        public Task<Response> Handle(RegistrarFacturaDto request, CancellationToken cancellationToken)
        {
            var tercero = Validator.Tercero;
            var usuario = Validator.UsuarioRegistroFactura;
            var nuevafactura = new Factura(usuario, Validator.CuentaxFactura.CuentaBancaria)
            {

                Tercero = tercero,
                Fechapago = request.FechaPago,
                Valor = request.Valor,
                Observaciones = request.Observaciones,
                Ri = request.Ri,
                ConceptoId = Validator.ConceptoFactura.Id,
                TerceroId=tercero.Id,
                Concepto = Validator.ConceptoFactura,
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
        public long Valor { get; set; }
        public string? Observaciones { get; set; }
        public DateTime FechaPago { get; set; }
        public string ? Ri { get; set; }
        public Guid CuentaBancariadId { get; set; }
        public Guid ConceptoFacturaId { get; set; }
        public RegistrarFacturaDto()
        {

        }
    }
    public class RegistrarFacturaDtoValidator : AbstractValidator<RegistrarFacturaDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Usuario UsuarioRegistroFactura;
        public Tercero Tercero;
        public ConceptoFactura ConceptoFactura;
        public CuentasBancariasxFactura CuentaxFactura;
        public RegistrarFacturaDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(e => e.UsuarioRegistroFacturaId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage($"El usuario que registro factura es obligatorio.")
                .Must(ExisteUsuario).WithMessage($"El usuario suministrado no fue encontrado en el sistema.");

            RuleFor(e => e.TerceroId.Value).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage($"El tercero(CodigoAN8) es obligatorio factura es obligatorio.")
                .Must(ExisteTercero).WithMessage($"El tercero suministrado no fue encontrado en el sistema.");

            RuleFor(e => e.Valor).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El valor de la factura es obligatorio.")
                .GreaterThanOrEqualTo(1000).WithMessage("El valor de la factura debe ser mayor a 1000.");

            RuleFor(e => e.FechaPago).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("No se encontro fecha de pago. factura.")
                .Must(ValidarFecha).WithMessage($"La fecha de la factura no puede ser mayor a la fecha actual.");

            RuleFor(e => e.ConceptoFacturaId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El concepto de factura es obligatorio.")
                .Must(ExisteConceptoFactura).WithMessage($"El concepto de factura no fue encontrado en el sistema.");

            When(e => ConceptoFactura != null, () =>
            {
                When(e => ConceptoFactura.Concepto == "RI", () =>
                {
                    RuleFor(e => e.Ri).Cascade(CascadeMode.StopOnFirstFailure)
                      .NotEmpty().WithMessage("Para el concepto RI, el numero RI es obligatorio.");
                });
            });

            RuleFor(e => e.CuentaBancariadId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("La entidad bancaria es obligatoria.")
                .Must(ExisteCuenta).WithMessage($"El cuenta de entidad no fue encontrado en el sistema.");

        }
        private bool ValidarFecha(DateTime fecha)
        {
           if(fecha > DateTime.Now) { return false; } else { return true; }
        }
        private bool ExisteCuenta(Guid id)
        {
            CuentaxFactura = _unitOfWork.GenericRepository<CuentasBancariasxFactura>().FindFirstOrDefault(e => e.Id == id);
            return CuentaxFactura != null;
        }
        private bool ExisteConceptoFactura(Guid id)
        {
            ConceptoFactura = _unitOfWork.GenericRepository<ConceptoFactura>().FindFirstOrDefault(e => e.Id == id);
            return ConceptoFactura != null;
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
