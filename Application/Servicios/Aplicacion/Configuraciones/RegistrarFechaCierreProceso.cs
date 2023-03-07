using Domain.Aplicacion;
using Domain.Aplicacion.EntidadesConfiguracion;
using Domain.Contracts;
using Domain.Entities;
using Domain.Extensions;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.Aplicacion.Configuraciones
{
    public class RegistrarFechaCierre : IRequestHandler<RegistrarFechaCierreDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RegistrarFechaCierreDtoValidator _validator;

        public RegistrarFechaCierreDtoValidator Validator { get; }

        public RegistrarFechaCierre(IUnitOfWork unitOfWork, IValidator<RegistrarFechaCierreDto> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator as RegistrarFechaCierreDtoValidator;
        }
        public Task<Response> Handle(RegistrarFechaCierreDto request, CancellationToken cancellationToken)
        {
            var usuarioconfiguro = _validator.UsuarioConfiguro;

            var configuracionProcesoNueva = new ConfiguracionProcesoNotasContables(request.Año,request.Mes,usuarioconfiguro)
            {
                FechaCierre=DateTime.Now,
                Año=request.Año,Mes=request.Mes,
                ProcesoNotaContable = Validator.Proceso,
                ProcesoId = request.ProcesoId,
                Id = Guid.NewGuid(), 

            };

            _unitOfWork.GenericRepository<ConfiguracionProcesoNotasContables>().Add(configuracionProcesoNueva); 
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Data = configuracionProcesoNueva,
                Mensaje = $"El proceso  {Validator.Proceso.NombreProceso} se ha cerrado a la fecha de: {DateTime.Now}."
            });
        }
    }
    public class RegistrarFechaCierreDto : IRequest<Response>
    {
        public int Mes { get; set; }
        public int Año { get; set; }
        public DateTime FechaManipularCierre { get; set; }
        public Guid UsuarioId { get; set; }
        public Guid ProcesoId { get; set; }
        public RegistrarFechaCierreDto()
        {

        }
        public RegistrarFechaCierreDto(int año,int mes)
        {
            FechaManipularCierre = new  DateTime(año, mes,01);

        }
    }
    public class RegistrarFechaCierreDtoValidator : AbstractValidator<RegistrarFechaCierreDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ConfiguracionProcesoNotasContables ConfiguracionProceso;
        public Usuario UsuarioConfiguro;
        public Usuario UsuarioVerificador;
        public Proceso Proceso;
        public RegistrarFechaCierreDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {


            RuleFor(bdu => bdu.ProcesoId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El proceso es obligatorio.")
                .Must(ExisteProceso).WithMessage($"El proceso no fue encontrado en el sistema.");
               

            When(e => Proceso != null, () =>
            {
                RuleFor(bdu => bdu.FechaManipularCierre).Cascade(CascadeMode.StopOnFirstFailure)
                .Must(ExisteConfiguracionProceso).WithMessage($"El proceso suministrado ya se encuentra cerrado en el mes establecido.");

                RuleFor(bdu => bdu.UsuarioId).Cascade(CascadeMode.StopOnFirstFailure)
                            .NotEmpty().WithMessage("El usuario es obligatorio.")
                            .Must(ExisteUsuario).WithMessage($"El usuario suministrado no fue encontrado en el sistema.")
                            .Must(RolUsuario).WithMessage($"El usuario no tiene permisos para realizar cierres en notas contables en proceso seleccionado.")
                            .Must(ValidarProcesoNotaContable).WithMessage($"El proceso no recibe notas contables."); ;

            });
        }
        private bool RolUsuario(Guid id)
        {
            switch (UsuarioConfiguro.Rol)
            {
                case Rol.Administrador:
                    return true;break;
                case Rol.AdministradorNotaContable :
                    if (Proceso.Id == UsuarioConfiguro.ProcesoId) return true; else return false; break;
                default:
                    return false;break;
            }
        }
        public bool ValidarProcesoNotaContable(Guid id)
        {
            
            UsuarioVerificador = _unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.ProcesoId==id);
            if(UsuarioVerificador != null)
            {
                switch (UsuarioVerificador.Rol)
                {
                    case Rol.AdministradorNotaContable:
                    case Rol.Verificadordenotascontables:
                        if (Proceso.Id == UsuarioVerificador.ProcesoId) return true; else return false;break;
                    default:
                        return false;break;
                }
            }
            else { return false; }
            
        }
        private bool ExisteProceso(Guid id)
        {
            Proceso = _unitOfWork.GenericRepository<Proceso>().FindFirstOrDefault(e => e.Id == id);
            return Proceso != null;
        }
        private bool ExisteConfiguracionProceso(DateTime fechamanipular)
        {
            ConfiguracionProceso = _unitOfWork.GenericRepository<ConfiguracionProcesoNotasContables>()
                .FindFirstOrDefault(e => e.ProcesoId==Proceso.Id && e.Año==fechamanipular.Year && e.Mes==fechamanipular.Month);
            return ConfiguracionProceso == null;
        }
        private bool ExisteUsuario(Guid id)
        {
            UsuarioConfiguro = _unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == id);
            return UsuarioConfiguro != null;
        }
    }
}
