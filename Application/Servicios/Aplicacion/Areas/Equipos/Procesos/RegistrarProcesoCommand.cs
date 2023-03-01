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

namespace Application.Servicios.Aplicacion.Areas.Equipos.Procesos
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
            var equipo = Validator.Equipo;
            var nuevoproceso = new Proceso(request.NombreProceso,null)
            {
                Id = Guid.NewGuid(),
                Equipo = equipo,
            };

            _unitOfWork.GenericRepository<Proceso>().Add(nuevoproceso);
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Data = nuevoproceso,
                Mensaje = $"El proceso ha sido registrado con exito."
            });
        }
    }
    public class RegistrarProcesoDto : IRequest<Response>
    {
        public string NombreProceso { get; set; }
        public Guid EquipoId { get; set; }
        public Guid UsuarioId { get; set; }
        public RegistrarProcesoDto()
        {
            EquipoId = Guid.NewGuid();
        }
        public RegistrarProcesoDto(string nombreProceso, Guid equipoId)
        {
            NombreProceso = nombreProceso;
            EquipoId = equipoId;
        }
    }
    public class RegistrarProcesoDtoValidator : AbstractValidator<RegistrarProcesoDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Equipo Equipo;
        public Usuario Usuario;
        public RegistrarProcesoDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(e => e.NombreProceso).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El nombre del proceso no puede ser vacio.")
                .Length(5, 15).WithMessage("El nombre del proceso debe tener entre 5 y 15 caracteres.");
            RuleFor(e => e.EquipoId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Debe seleccionar un equipo para el proceso.")
                .Must(ExisteEquipo).WithMessage($"El equipo no fue encontrado.");

            RuleFor(bdu => bdu.UsuarioId).Cascade(CascadeMode.StopOnFirstFailure)
              .NotEmpty().WithMessage("El usuario es obligatorio.")
              .Must(ExisteUsuarioAdmin).WithMessage("El usuario no fue encontrado en el sistema.")
              .Must(RolUsuario).WithMessage("El usuario no tiene permiso para registrar procesos.");

        }
        private bool RolUsuario(Guid id)
        {
            if (Usuario.Rol != Rol.Administrador) { return false; } else { return true; }
        }
        private bool ExisteUsuarioAdmin(Guid id)
        {
            Usuario = _unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == id);
            return Usuario != null;
        }
        private bool ExisteEquipo(Guid id)
        {
            Equipo = _unitOfWork.GenericRepository<Equipo>().FindBy(e => e.Id == id).FirstOrDefault();
            return Equipo != null;
        }
    }
}
