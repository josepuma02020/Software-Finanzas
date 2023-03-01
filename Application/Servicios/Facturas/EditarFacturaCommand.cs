using Application.Servicios.Bases;
using Domain.Aplicacion.Entidades;
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
    public class EditarFacturaCommand : IRequestHandler<EditarFacturaDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public EditarFacturaDtoValidator Validator { get; }

        public EditarFacturaCommand(IUnitOfWork unitOfWork, IValidator<EditarFacturaDto> validator)
        {
            _unitOfWork = unitOfWork;
            Validator = validator as EditarFacturaDtoValidator;
        }
        public Task<Response> Handle(EditarFacturaDto request, CancellationToken cancellationToken)
        {
            var tercero = Validator.Tercero;
            var usuario = Validator.UsuarioEditor;
            var factura = Validator.Factura;


            _unitOfWork.GenericRepository<Factura>().Edit(factura.
                EditarFactura(usuario,tercero,request.Observaciones,request.FechaPago,request.Ri,Validator.CuentaxFactura.CuentaBancaria,Validator.ConceptoFactura));
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Data = factura,
                Mensaje = $"La factura se editó correctamente."
            });
        }
    }
    public class EditarFacturaDto : IRequest<Response>
    {
        public Guid UsuarioEditor { get; set; }
        public Guid? TerceroId { get; set; }
        public long Valor { get; set; }
        public string? Observaciones { get; set; }
        public DateTime FechaPago { get; set; }
        public string? Ri { get; set; }
        public Guid CuentaFacturaId { get; set; }
        public Guid ConceptoFacturaId { get; set; }
        public Guid FacturaId { get; set; }
        public EditarFacturaDto()
        {

        }
    }
    public class EditarFacturaDtoValidator : AbstractValidator<EditarFacturaDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Usuario UsuarioEditor;
        public Tercero Tercero;
        public ConceptoFactura ConceptoFactura;
        public CuentasBancariasxFactura CuentaxFactura;
        public Factura Factura;
        public EditarFacturaDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {

            RuleFor(e => e.FacturaId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage($"El id de la factura a editar es obligatoria.")
                .Must(ExisteFactura).WithMessage($"La factura suministrada no fue encontrada en el sistema.");

            RuleFor(e => e.UsuarioEditor).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage($"El usuario que registro factura es obligatorio.")
                .Must(ExisteUsuario).WithMessage($"El usuario suministrado no fue encontrado en el sistema.");

            When(a => Factura != null && UsuarioEditor != null, () =>
            {
                RuleFor(a => new { request = a }).Custom((IdUsuarioRechazador, context) =>
                {
                    Factura.PuedeEditar(UsuarioEditor).ToValidationFailure(context);
                });
            });

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

            RuleFor(e => e.CuentaFacturaId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("La cuenta bancaria es obligatoria.")
                    .Must(ExisteCuenta).WithMessage($"La cuenta bancaria suministrada no fue encontrada en el sistema.");

        }
        private bool ValidarFecha(DateTime fecha)
        {
            if (fecha > DateTime.Now) { return false; } else { return true; }
        }
        private bool ExisteFactura(Guid id)
        {
            Factura = _unitOfWork.GenericRepository<Factura>().FindFirstOrDefault(e => e.Id == id);
            return Factura != null;
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
            UsuarioEditor = _unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == id);
            return UsuarioEditor != null;
        }
        private bool ExisteTercero(Guid id)
        {
            Tercero = _unitOfWork.GenericRepository<Tercero>().FindFirstOrDefault(e => e.Id == id);
            return Tercero != null;
        }

    }
}
