using Domain.Base;
using Domain.Clases;
using Domain.Contracts;
using Domain.Entities;
using Domain.Extensions;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.Documentos
{
    public class VerificarDocumentoCommand : IRequestHandler<VerificarDocumentoDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RevisarDocumentoValidator _validator;

        public VerificarDocumentoCommand(IUnitOfWork unitOfWork, IValidator<VerificarDocumentoDto> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator as RevisarDocumentoValidator;
        }
        public Task<Response> Handle(VerificarDocumentoDto request, CancellationToken cancellationToken)
        {
            var documento = _validator.Documento;
            var usuarioVerifica = _validator.UsuarioVerificador;
            _unitOfWork.GenericRepository<BaseEntityDocumento>().Edit(documento.SetVerificador(usuarioVerifica));
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Mensaje = $"El documento se ha aprobado con exito"
            });
        }
    }
    public class VerificarDocumentoDto : IRequest<Response>
    {
        public Guid IdDocumento { get; set; }
        public Guid IdUsuarioVerificador { get; set; }
        public VerificarDocumentoDto()
        {

        }
        public VerificarDocumentoDto(Guid id, Guid verificador)
        {
            IdDocumento = id;
            IdUsuarioVerificador = verificador;
        }
    }
    public class RevisarDocumentoValidator : AbstractValidator<VerificarDocumentoDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public BaseEntityDocumento Documento { get; private set; }
        public Usuario UsuarioVerificador { get; private set; }

        public RevisarDocumentoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(bdu => bdu.IdDocumento).Must(ExisteDocumento).WithMessage($"El documento no existe");
            RuleFor(bdu => bdu.IdUsuarioVerificador).Must(ExisteUsuario).WithMessage($"El usuario no existe");
        }
        private bool ExisteDocumento(Guid id)
        {
            Documento = _unitOfWork.GenericRepository<BaseEntityDocumento>().FindBy(e => e.Id == id).FirstOrDefault();
            return Documento != null;
        }
        private bool ExisteUsuario(Guid id)
        {
            UsuarioVerificador = _unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e=> e.Id== id);
            return UsuarioVerificador != null;
        }
    }
}
