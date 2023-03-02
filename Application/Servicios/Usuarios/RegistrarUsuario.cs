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

namespace Application.Servicios.Usuarios
{
    public class RegistrarUsuario : IRequestHandler<RegistrarUsuarioDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegistrarUsuarioDtoValidator Validator { get; }

        public RegistrarUsuario(IUnitOfWork unitOfWork, IValidator<RegistrarUsuarioDto> validator)
        {
            _unitOfWork = unitOfWork;
            Validator = validator as RegistrarUsuarioDtoValidator;
        }
        public Task<Response> Handle(RegistrarUsuarioDto request, CancellationToken cancellationToken)
        {
            var Area = Validator.Area;
            var Equipo = Validator.Equipo;
            if (Area == null)
            {
                var nuevaArea = new Area(){ CodigoDependencia = request.CodigoDependencia, NombreArea = request.NombreArea,};
                _unitOfWork.GenericRepository<Area>().Add(nuevaArea);
                _unitOfWork.Commit();
                Area = nuevaArea;
                var nuevoEquipo = new Equipo(null)
                {
                    Area = nuevaArea,
                    
                    NombreEquipo = request.NombreEquipo,
                    CodigoEquipo = request.CodigoEquipo, 
                };
                _unitOfWork.GenericRepository<Equipo>().Add(nuevoEquipo);
                _unitOfWork.Commit();
                Equipo = nuevoEquipo;
            }
            else
            {
                if(Equipo == null)
                {
                    var nuevoEquipo = new Equipo(null)
                    {
                        Area = Area,
                        NombreEquipo = request.NombreEquipo,
                        CodigoEquipo = request.CodigoEquipo,
                    };
                    _unitOfWork.GenericRepository<Equipo>().Add(nuevoEquipo);
                    _unitOfWork.Commit();
                    Equipo = nuevoEquipo;
                }
            }
            var nuevousuario = new Usuario(null)
            {
            
            Equipo=Equipo,
            Nombre=request.Nombre,
            Email=request.Email,
            EquipoId=Equipo.Id,
            Identificacion=request.Identificacion,
            Id = Guid.NewGuid(),
            };
            _unitOfWork.GenericRepository<Usuario>().Add(nuevousuario);
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Data = nuevousuario,
                Mensaje = $"El usuario {request.Nombre} se registró correctamente."
            });
        }
    }
    public class RegistrarUsuarioDto : IRequest<Response>
    {
        public string  Identificacion { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string CodigoDependencia { get; set; }
        public string NombreArea { get; set; }
        public string NombreEquipo { get; set; }
        public string? CodigoEquipo { get; set; }
        public RegistrarUsuarioDto()
        {

        }
    }
    public class RegistrarUsuarioDtoValidator : AbstractValidator<RegistrarUsuarioDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Area Area;
        public Equipo Equipo;
        public RegistrarUsuarioDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(e => e.Nombre).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El nombre del Usuario es obligatorio.")
                .Length(4, 40).WithMessage("El nombre del Usuario debe tener mas de 4 caracteres.");

            RuleFor(e => e.Identificacion).NotEmpty().WithMessage("El campo identificación del Usuario es obligatorio.");

            RuleFor(e => e.CodigoDependencia).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El codigo del area  del Usuario es obligatorio.")
                .Must(ValidarArea);

            RuleFor(e => e.NombreEquipo).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El nombre del equipo del Usuario es obligatorio.")
                .Must(ValidarEquipo);

            RuleFor(e => e.Email).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El campo Email es obligatorio.")
                .Length(5, 60).WithMessage("El email del Usuario debe tener mas de 5 caracteres.");
        }
        public bool ValidarArea(string codigo)
        {
            Area = _unitOfWork.GenericRepository<Area>().FindFirstOrDefault(e => e.CodigoDependencia == codigo);
            return true;
        }
        public bool ValidarEquipo(string codigo)
        {
            Equipo = _unitOfWork.GenericRepository<Equipo>().FindFirstOrDefault(e => e.NombreEquipo == codigo);
            return true;
        }

    }
}
