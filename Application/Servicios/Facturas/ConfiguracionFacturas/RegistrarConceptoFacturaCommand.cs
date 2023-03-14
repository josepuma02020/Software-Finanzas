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

namespace Application.Servicios.Facturas.ConfiguracionFacturas
{
    public class RegistrarConceptoFacturaCommand : IRequestHandler<RegistrarConceptoFacturaDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegistrarConceptoFacturaDtoValidator Validator { get; }

        public RegistrarConceptoFacturaCommand(IUnitOfWork unitOfWork, IValidator<RegistrarConceptoFacturaDto> validator)
        {
            _unitOfWork = unitOfWork;
            Validator = validator as RegistrarConceptoFacturaDtoValidator;
        }
        public Task<Response> Handle(RegistrarConceptoFacturaDto request, CancellationToken cancellationToken)
        {
            var nuevoConceptoFactura = new ConceptoFactura(Validator.Usuario)
            {
                Concepto = request.ConceptoFactura,
                Descripcion = request.Descripcion,
            };
            
            _unitOfWork.GenericRepository<ConceptoFactura>().Add(nuevoConceptoFactura);
            _unitOfWork.Commit();

            return Task.FromResult(new Response
            {
                //Data = nuevoConceptoFactura,
                Mensaje = $"El concepto de factura se registró correctamente."
            });
        }
    }
    public class RegistrarConceptoFacturaDto : IRequest<Response>
    {
        public string ConceptoFactura { get; set; }
        public string? Descripcion { get; set; }
        public Guid UsuarioId { get; set; }
        public RegistrarConceptoFacturaDto()
        {

        }
    }
    public class RegistrarConceptoFacturaDtoValidator : AbstractValidator<RegistrarConceptoFacturaDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Usuario Usuario;
        public RegistrarConceptoFacturaDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(bdu => bdu.ConceptoFactura).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage($"El concepto de factura es obligatorio.")
                .Length(2, 25).WithMessage($"El concepto debe tener de 1 a 15 caracteres.");

            RuleFor(bdu => bdu.UsuarioId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El usuario es obligatorio.")
                .Must(ExisteUsuario).WithMessage("El usuario no fue encontrado en el sistema.")
                .Must(RolUsuario).WithMessage("El usuario no tiene permisos para agregar conceptos de factura.");
        }
        private bool ExisteUsuario(Guid id)
        {
            Usuario = _unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == id);
            return Usuario != null;
        }
        private bool RolUsuario(Guid id)
        {
            bool valido = false;
            if(Usuario.Rol == Rol.AdministradorFactura || Usuario.Rol == Rol.Administrador) { valido = true; }
            return valido;
        }

    }
}
