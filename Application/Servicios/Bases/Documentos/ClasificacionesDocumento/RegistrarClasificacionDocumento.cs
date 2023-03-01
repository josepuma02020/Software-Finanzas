using Domain.Contracts;
using Domain.Documentos.ConfiguracionDocumentos;
using Domain.Entities;
using Domain.Extensions;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.Bases.Documentos.ClasificacionesDocumento
{
    public class RegistrarClasificacionDocumento : IRequestHandler<RegistrarClasificacionDocumentoDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegistrarClasificacionDocumentoDtoValidator Validator { get; }

        public RegistrarClasificacionDocumento(IUnitOfWork unitOfWork, IValidator<RegistrarClasificacionDocumentoDto> validator)
        {
            _unitOfWork = unitOfWork;
            Validator = validator as RegistrarClasificacionDocumentoDtoValidator;
        }
        public Task<Response> Handle(RegistrarClasificacionDocumentoDto request, CancellationToken cancellationToken)
        {
            var nuevaclasificacion = new ClasificacionDocumento(Validator.Usuario)
            {
                Descripcion = request.Descripcion,
                Id = Guid.NewGuid(),
                ClasificacionProceso = request.ProcesoDocumento.Value,
            };

            _unitOfWork.GenericRepository<ClasificacionDocumento>().Add(nuevaclasificacion);
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Data = nuevaclasificacion,
                Mensaje = $"La clasificacion de documento se ha registrado correctamente."
            });
        }
    }
    public class RegistrarClasificacionDocumentoDto : IRequest<Response>
    {
        public string Descripcion { get; set; }
        public ProcesosDocumentos? ProcesoDocumento { get; set; }
        public Guid UsuarioId { get; set; }
        public RegistrarClasificacionDocumentoDto()
        {

        }
    }
    public class RegistrarClasificacionDocumentoDtoValidator : AbstractValidator<RegistrarClasificacionDocumentoDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Usuario Usuario;
        public RegistrarClasificacionDocumentoDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(e => e.ProcesoDocumento).NotEmpty().WithMessage("El proceso de documento no debe ser vacio.");

            RuleFor(e => e.Descripcion).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("La descripcion no puede ser nula.")
                .Length(5, 20).WithMessage("La descripcion debe tener de 5 a 20 caracteres.");

            RuleFor(bdu => bdu.UsuarioId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El usuario es obligatorio.")
                .Must(ExisteUsuarioAdmin).WithMessage("El usuario no fue encontrado en el sistema.");

            When(e => Usuario != null, () =>
            {
                When(e => e.ProcesoDocumento == ProcesosDocumentos.Facturas, () =>
                {
                    RuleFor(e=>e.UsuarioId).Must(RolAdminFactura).WithMessage("El usuario no tiene permitido registrar clasificaciones de documentos en facturas.");
                });
                When(e => e.ProcesoDocumento == ProcesosDocumentos.NotasContable, () =>
                {
                    RuleFor(e => e.UsuarioId).Must(RolAdminNota).WithMessage("El usuario no tiene permitido registrar clasificaciones de documentos en notas contables.");
                });
            });
        }
        private bool ExisteUsuarioAdmin(Guid id)
        {
            Usuario = _unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == id);
            return Usuario != null;
        }
        private bool RolAdminFactura(Guid id)
        {
            bool valido = false;
            if (Usuario.Rol == Rol.Administrador || Usuario.Rol == Rol.AdministradorFactura) { valido = true; }
            return valido;
        }
        private bool RolAdminNota(Guid id)
        {
            bool valido = false;
            if (Usuario.Rol == Rol.Administrador || Usuario.Rol == Rol.AdministradorNotaContable) { valido = true; }
            return valido;
        }

    }
}
