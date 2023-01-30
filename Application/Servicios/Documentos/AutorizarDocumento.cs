using Domain.Base;
using Domain.Clases;
using Domain.Contracts;
using Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.Documentos
{
    public class AutorizarDocumentoCommand : IRequestHandler<AutorizarDocumentoDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RevisarDocumentoValidator _validator;

        public AutorizarDocumentoCommand(IUnitOfWork unitOfWork, IValidator<AutorizarDocumentoDto> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator as RevisarDocumentoValidator;
        }
        public Task<Response> Handle(AutorizarDocumentoDto request, CancellationToken cancellationToken)
        {
            ValidarPropiedasparaRevisionDocumento(request);
            var documento = _validator.Documento;
            var usuarioAutoriza = _validator.UsuarioVerificador;
            _unitOfWork.GenericRepository<BaseEntityDocumento>().Edit(documento.SetAutorizador(usuarioAutoriza));
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Mensaje = $"El documento se ha aprobado con exito"
            });
        }

        private void ValidarPropiedasparaRevisionDocumento(AutorizarDocumentoDto request)
        {
            VerificarDocumentoDto DtoValidador = new VerificarDocumentoDto(request.IdDocumento, request.IdUsuarioVerificador);
            var respuesta = _validator.Validate(DtoValidador);
            if (!respuesta.IsValid) throw new ValidationException("Los datos para aprobar el documento no son validos");
        }
    }
    public class AutorizarDocumentoDto : IRequest<Response>
    {
        public Guid IdDocumento { get; set; }
        public Guid IdUsuarioVerificador { get; set; }
        public AutorizarDocumentoDto()
        {
        }
        public AutorizarDocumentoDto(Guid id, Guid verificador)
        {
            IdDocumento = id;
            IdUsuarioVerificador = verificador;
        }
    }

}
