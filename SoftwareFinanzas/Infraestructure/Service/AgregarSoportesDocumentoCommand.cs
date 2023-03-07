using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Application;
using Application.Servicios.Bases;
using Domain.Base;
using Domain.Contracts;
using Domain.Documentos;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace SoftwareFinanzas.Infraestructure.Service
{
    public class AgregarSoportesDocumentoCommand : IRequestHandler<AgregarSoportesDocumentoDto, Response>
    {
        private readonly IWebHostEnvironment _env;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AgregarSoportesDocumentoDtoValidator _validator;   

        public AgregarSoportesDocumentoCommand(IWebHostEnvironment env, IUnitOfWork unitOfWork, IValidator<AgregarSoportesDocumentoDto> validator)
        {
            _env = env;
            _unitOfWork = unitOfWork;
            _validator = validator as AgregarSoportesDocumentoDtoValidator;
        }

        public async Task<Response> Handle(AgregarSoportesDocumentoDto request, CancellationToken cancellationToken)
        {
            List<AppFile> listaSoportes = new List<AppFile>();
            foreach (IFormFile soporte in request.Soportes){
                var filePath = Path.Combine(_env.WebRootPath, "Soportes",_validator.Documento.Id.ToString(), soporte.FileName);
                AppFile appFile;
                using (var stream = File.Create(filePath))
                {
                    await soporte.CopyToAsync(stream);
                    appFile = new AppFile.AppFileBuilder()
                        .SetNombre(soporte.FileName)
                        .SetPath($"Soportes\\{request.DocumentoId}\\{soporte.FileName}")
                        .Build();
                }
                listaSoportes.Add(appFile);

            }
            _unitOfWork.GenericRepository<BaseEntityDocumento>().Edit(_validator.Documento.SetSoportes(listaSoportes,_validator.UsuarioCargoSoporte));
            _unitOfWork.Commit();
            return await Task.FromResult(new Response
            {
               
            });
        }
    }
    public class AgregarSoportesDocumentoDto : IRequest<Response>
    {
        public List<IFormFile> Soportes { get; set; }
        public Guid UsuarioQueSubioElArchivoId { get; set; }
        public Guid DocumentoId { get; set; }
    }
    public class AgregarSoportesDocumentoDtoValidator : AbstractValidator<AgregarSoportesDocumentoDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public BaseEntityDocumento Documento { get; set; }
        public Usuario UsuarioCargoSoporte { get; set; }
        public AgregarSoportesDocumentoDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidator();
        }
        private void SetUpValidator()
        {
            RuleFor(bdu => bdu.DocumentoId ).Cascade(CascadeMode.StopOnFirstFailure)
              .NotEmpty().WithMessage("El id del documento es obligatorio.")
              .Must(ExisteDocumento).WithMessage($"El id del documento suministrado no fué localizado en el sistema.")
              .Must(EstadoDocumento).WithMessage($"El documento no esta disponible para aprobación.");

            RuleFor(bdu => bdu.UsuarioQueSubioElArchivoId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El id de usuario es obligatorio.")
                .Must(ExisteUsuario).WithMessage($"El usuario suministrado no fué localizado en el sistema.");

            When(t => Documento != null && UsuarioCargoSoporte != null, () =>
            {
                RuleFor(a => new { request = a }).Custom((IdUsuarioRechazador, context) =>
                {
                    Documento.PuedeAgregarSoporte(UsuarioCargoSoporte).ToValidationFailure(context);
                });
            });

            RuleForEach(x => x.Soportes).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("La lista de soportes es obligatoria")
                .Must(ValidarTipoSoporte).WithMessage("Solo se permiten soportes pdf, jpg, png y jpeg.");
        }
        private bool ExisteUsuario(Guid id)
        {
            UsuarioCargoSoporte = _unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == id);
            return UsuarioCargoSoporte != null;
        }
        private bool ExisteDocumento(Guid id)
        {
            Documento = _unitOfWork.GenericRepository<NotaContable>().FindFirstOrDefault(e => e.Id == id);
            return Documento != null;
        }
        private bool EstadoDocumento(Guid id)
        {
            if (Documento.EstadoDocumento == Domain.Base.EstadoDocumento.Revision) { return true; } else { return false; }
        }
        private bool ValidarTipoSoporte(IFormFile Soporte)
        {
            switch (Soporte.ContentType)
            {
                case "application/pdf":
                case "application/jpg":
                case "application/png":
                case "application/jpeg":return true; 
                default:return false;
            }
        }

    }
}
