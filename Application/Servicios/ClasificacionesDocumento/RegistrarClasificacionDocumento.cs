using Domain.Contracts;
using Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.ClasificacionesDocumento
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
            var nuevaclasificacion = new ClasificacionDocumento()
            {
            
            Id = Guid.NewGuid(),
            };


            _unitOfWork.GenericRepository<ClasificacionDocumento>().Add(nuevaclasificacion);
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Data = nuevaclasificacion,
                Mensaje = $"La usuario {request.Nombre}  se registró correctamente."
            });
        }
    }
    public class RegistrarClasificacionDocumentoDto : IRequest<Response>
    {
        public string Identificacion { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
    }
    public class RegistrarClasificacionDocumentoDtoValidator : AbstractValidator<RegistrarClasificacionDocumentoDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public RegistrarClasificacionDocumentoDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(e => e.Nombre).NotEmpty().Length(4, 15).WithMessage("El nombre del Usuario debe tener mas de 4 caracteres");
            RuleFor(e => e.Email).NotEmpty().WithMessage("El campo Email, es obligatorio");
            RuleFor(e => e.Identificacion).NotEmpty().WithMessage("El campo Identificacion, es obligatorio"); ;
        }

    }
}
