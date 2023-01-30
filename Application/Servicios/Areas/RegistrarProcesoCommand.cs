using Domain.Aplicacion;
using Domain.Contracts;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.Areas
{
    public class RegistrarProcesoCommand : IRequestHandler<RegistrarProcesoDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegistrarProcesoDtoValidator Validator { get; }

        public RegistrarProcesoCommand(IUnitOfWork unitOfWork, IValidator<RegistrarProcesoDto> validator)
        {
            _unitOfWork = unitOfWork;
            Validator = validator as RegistrarProcesoDtoValidator;
        }
        public Task<Response> Handle(RegistrarProcesoDto request, CancellationToken cancellationToken)
        {
            var nuevoproceso = new Proceso(request.NombreProceso, request.EquipoId)
            {
                Id = Guid.NewGuid(),
            };


            _unitOfWork.GenericRepository<Proceso>().Add(nuevoproceso);
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Data = nuevoproceso,
                Mensaje = $"El proceso {request.NombreProceso}  se registró correctamente."
            });
        }
    }
    public class RegistrarProcesoDto : IRequest<Response>
    { 
        public string NombreProceso { get; set; }
        public Guid EquipoId { get; set; }
        public RegistrarProcesoDto()
        {

        }
        public RegistrarProcesoDto(string nombreProceso,Guid equipoId)
        {
            NombreProceso = nombreProceso;
            EquipoId = equipoId;
        }
    }
    public class RegistrarProcesoDtoValidator : AbstractValidator<RegistrarProcesoDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public RegistrarProcesoDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(e => e.NombreProceso).NotEmpty().Length(5, 15);
            RuleFor(e => e.EquipoId).NotEmpty();
        }
    }
}
