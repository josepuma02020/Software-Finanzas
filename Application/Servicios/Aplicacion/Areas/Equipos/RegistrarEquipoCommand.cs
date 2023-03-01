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

namespace Application.Servicios.Aplicacion.Areas.Equipos
{
    public class RegistrarEquipoCommand : IRequestHandler<RegistrarEquipoDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegistrarEquipoDtoValidator Validator { get; }

        public RegistrarEquipoCommand(IUnitOfWork unitOfWork, IValidator<RegistrarEquipoDto> validator)
        {
            _unitOfWork = unitOfWork;
            Validator = validator as RegistrarEquipoDtoValidator;
        }
        public Task<Response> Handle(RegistrarEquipoDto request, CancellationToken cancellationToken)
        {
            var nuevoequipo = new Equipo(request.NombreEquipo, null)
            {
                Id = Guid.NewGuid(),
                Area = Validator.Area,
            };
            _unitOfWork.GenericRepository<Equipo>().Add(nuevoequipo);
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Data = nuevoequipo,
                Mensaje = $"El equipo {request.NombreEquipo} se registró correctamente."
            });
        }
    }
    public class RegistrarEquipoDto : IRequest<Response>
    {
        public string NombreEquipo { get; set; }
        public Guid AreaId { get; set; }
        public RegistrarEquipoDto()
        {

        }
        public RegistrarEquipoDto(string nombreEquipo, Guid areaId)
        {
            NombreEquipo = nombreEquipo;
            AreaId = areaId;
        }
    }
    public class RegistrarEquipoDtoValidator : AbstractValidator<RegistrarEquipoDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Area Area;
        public Usuario Usuario;
        public RegistrarEquipoDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(e => e.NombreEquipo).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El nombre del equipo no puede ser vacio.")
                .Length(5, 15).WithMessage("El nombre del equipo debe tener entre 5 y 15 caracteres.");
            RuleFor(e => e.AreaId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Se debe seleccionar un Area para el equipo.")
                .Must(ExisteArea).WithMessage($"El area no fue encontrada.");

        }
  
        private bool ExisteArea(Guid id)
        {
            Area = _unitOfWork.GenericRepository<Area>().FindBy(e => e.Id == id).FirstOrDefault();
            return Area != null;
        }
    }
}
