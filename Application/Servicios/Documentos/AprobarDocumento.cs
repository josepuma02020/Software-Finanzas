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
    public class AprobarDocumentoCommand : IRequestHandler<AprobarDocumentoDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RevisarDocumentoValidator _validator;

        public AprobarDocumentoCommand(IUnitOfWork unitOfWork, IValidator<AprobarDocumentoDto> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator as RevisarDocumentoValidator;
        }
        public Task<Response> Handle(AprobarDocumentoDto request, CancellationToken cancellationToken)
        {
            VerificarDocumentoDto DtoValidador = new VerificarDocumentoDto(request.IdDocumento, request.IdUsuarioVerificador);
            var respuesta = _validator.Validate(DtoValidador);
            if (!respuesta.IsValid) throw new ValidationException("Los datos para aprobar el documento no son validos");


            var documento = _validator.Documento;
            var usuarioaprueba = _validator.UsuarioVerificador;
            _unitOfWork.GenericRepository<BaseEntityDocumento>().Edit(documento.SetAprobador(usuarioaprueba));
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Mensaje = $"El documento se ha aprobado con exito"
            });
        }
    }
    public class AprobarDocumentoDto : IRequest<Response>
    {
        public Guid IdDocumento { get; set; }
        public Guid IdUsuarioVerificador { get; set; }
        public AprobarDocumentoDto()
        {
            
        }
        public AprobarDocumentoDto(Guid id, Guid verificador)
        {
            IdDocumento = id;
            IdUsuarioVerificador = verificador;
        }
    }
}
