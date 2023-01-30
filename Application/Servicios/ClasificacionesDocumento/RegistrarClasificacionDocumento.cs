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
                Descripcion = request.Descripcion,
                Id = Guid.NewGuid(),
                ClasificacionProceso = request.ClasificacionProceso.Value,
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
        public ProcesosDocumentos ? ClasificacionProceso { get; set; }
        public RegistrarClasificacionDocumentoDto()
        {

        }
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
            RuleFor(e => e.ClasificacionProceso).NotNull();
            RuleFor(e => e.Descripcion).NotEmpty().Length(5, 20).WithMessage("La descripcion es muy corta"); ;
        }

    }
}
