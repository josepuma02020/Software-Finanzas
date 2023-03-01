using Application.Servicios.Aplicacion.Areas;
using Application.Servicios.Bases.Documentos.ClasificacionesDocumento;
using Domain.Contracts;
using Domain.Documentos.ConfiguracionDocumentos;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Validators;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.Bases.Documentos.TiposDocumento
{
    public class RegistrarTipoDocumento : IRequestHandler<RegistrarTipoDocumentoDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        public RegistrarTipoDocumentoDtoValidator Validator { get; }


        public RegistrarTipoDocumento(IUnitOfWork unitOfWork, IValidator<RegistrarTipoDocumentoDto> validator)
        {
            _unitOfWork = unitOfWork;
            Validator = validator as RegistrarTipoDocumentoDtoValidator;
        }
        public Task<Response> Handle(RegistrarTipoDocumentoDto request, CancellationToken cancellationToken)
        {
            var nuevoTipoDocumento = new TipoDocumento(Validator.Usuario)
            {
                DescripcionTipoDocumento = request.DescripcionTipoDocumento,
                Id = Guid.NewGuid(),
            };


            _unitOfWork.GenericRepository<TipoDocumento>().Add(nuevoTipoDocumento);
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Data = nuevoTipoDocumento,
                Mensaje = $"El Tipo de documento se ha registrado correctamente."
            });
        }
    }
    public class RegistrarTipoDocumentoDto : IRequest<Response>
    {
        public string? CodigoTipoDocumento { get; set; }
        public string? DescripcionTipoDocumento { get; set; }
        public Guid UsuarioId { get; set; }
        public RegistrarTipoDocumentoDto()
        {

        }
    }
    public class RegistrarTipoDocumentoDtoValidator : AbstractValidator<RegistrarTipoDocumentoDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public TipoDocumento TipoDocumento;
        public Usuario Usuario;
        public RegistrarTipoDocumentoDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }
        private void SetUpValidators()
        {
            RuleFor(e => e.DescripcionTipoDocumento).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("La descripcion del documento no puede ser nula.")
                .Length(5, 50).WithMessage("La descripcion del documento debe tener mas de 5 caracteres.");

            RuleFor(e => e.CodigoTipoDocumento).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().WithMessage("El codigo del documento no puede ser vacío.")
            .Length(2, 10).WithMessage("El codigo debe tener entre 2 y 10 caracteres.")
            .Must(NoExisteTipoDocumento)
                .WithMessage("El tipo de documento que intenta registrar ya existe.");

            RuleFor(bdu => bdu.UsuarioId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El usuario es obligatorio.")
                .Must(ExisteUsuarioAdmin).WithMessage("El usuario no fue encontrado en el sistema.")
                .Must(RolUsuario).WithMessage("Solo el administrador puede registrar tipos de documentos.");
        }

        private bool ExisteUsuarioAdmin(Guid id)
        {
            Usuario = _unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == id);
            return Usuario != null;
        } 
        private bool RolUsuario(Guid id)
        {
            bool valido = false;
            if (Usuario.Rol == Rol.Administrador) { valido = true; }
            return valido;
        }
        private bool NoExisteTipoDocumento(string codigotipo)
        {
            TipoDocumento = _unitOfWork.GenericRepository<TipoDocumento>()
               .FindFirstOrDefault(e => e.CodigoTipoDocumento == codigotipo);
            return TipoDocumento == null;
        }
    }
}
