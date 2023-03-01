using Domain.Aplicacion.EntidadesConfiguracion;
using Domain.Contracts;
using Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.Aplicacion.Areas
{
    public class RegistrarAreaCommand : IRequestHandler<RegistrarAreaDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegistrarAreaDtoValidator Validator { get; }

        public RegistrarAreaCommand(IUnitOfWork unitOfWork, IValidator<RegistrarAreaDto> validator)
        {
            _unitOfWork = unitOfWork;
            Validator = validator as RegistrarAreaDtoValidator;
        }
        public Task<Response> Handle(RegistrarAreaDto request, CancellationToken cancellationToken)
        {
            var nuevaarea = new Area(request.Nombre,null)
            {
                Id = Guid.NewGuid(),
                CodigoDependencia = request.CodigoDependencia,
            };
            _unitOfWork.GenericRepository<Area>().Add(nuevaarea);
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Data = nuevaarea,
                Mensaje = $"El area {request.Nombre} se registró correctamente."
            });
        }
    }
    public class RegistrarAreaDto : IRequest<Response>
    {
        public string Nombre { get; set; }
        public string CodigoDependencia { get; set; }
        public RegistrarAreaDto()
        {

        }
        public RegistrarAreaDto(string nombreArea)
        {
            Nombre = nombreArea;
        }
        public RegistrarAreaDto(string nombreArea, string codigoDependencia)
        {
            Nombre = nombreArea;
            CodigoDependencia = codigoDependencia;
        }
    }
    public class RegistrarAreaDtoValidator : AbstractValidator<RegistrarAreaDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Usuario Usuario;
        public RegistrarAreaDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(e => e.Nombre).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El nombre del area no deberia estar vacio.")
                .Length(5, 15).WithMessage("El nombre debe tener de 5 a 15 caracteres.");
            RuleFor(e => e.CodigoDependencia).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El Area necesita un codigo de dependencia.")
                .Length(3, 15).WithMessage("El codigo de dependencia debe tener de 5 a 15 caracteres.");

            
        }
     
    }
}

