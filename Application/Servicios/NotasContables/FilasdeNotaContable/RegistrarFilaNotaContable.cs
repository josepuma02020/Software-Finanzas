using Application.Servicios.Bases;
using Domain.Aplicacion;
using Domain.Aplicacion.Entidades;
using Domain.Aplicacion.Entidades.CuentasContables;
using Domain.Aplicacion.EntidadesConfiguracion;
using Domain.Base;
using Domain.Contracts;
using Domain.Documentos;
using Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.NotasContables.FilasdeNotaContable
{
    public class RegistrarFilaNotaContable : IRequestHandler<RegistrarFilaNotaContableDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegistrarFilaNotaContableDtoValidator Validator { get; }

        public RegistrarFilaNotaContable(IUnitOfWork unitOfWork, IValidator<RegistrarFilaNotaContableDto> validator)
        {
            _unitOfWork = unitOfWork;
            Validator = validator as RegistrarFilaNotaContableDtoValidator;
        }
        public Task<Response> Handle(RegistrarFilaNotaContableDto request, CancellationToken cancellationToken)
        {
            var cuenta = Validator.CuentaContable;
            var notacontable = Validator.NotaContable;
            var usuario = Validator.Usuario;
            var terceroan8 = Validator.TerceroAN8;
            var tercerolm = Validator.TerceroLM;
            var nuevoregistro = new Registrodenotacontable(usuario, terceroan8,tercerolm, cuenta,notacontable)
            {
               Fecha=request.Fecha,
               Importe=request.Importe,
               Id = Guid.NewGuid(),
            };


            _unitOfWork.GenericRepository<Registrodenotacontable>().Add(nuevoregistro);
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
               // Data = nuevoregistro,
                Mensaje = $"La nota contable  se registró correctamente."
            });
        }
    }
    public class RegistrarFilaNotaContableDto : IRequest<Response>
    {
        public Guid TerceroId { get; set; }
        public Guid CuentaContableId { get; set; }
        public DateTime? Fecha { get; set; }
        public long Importe { get; set; }
        public Guid? TerceroLMId { get; set; }
        public Guid UsuarioId { get; set; }
        public RegistrarFilaNotaContableDto()
        {

        }
    }
    public class RegistrarFilaNotaContableDtoValidator : AbstractValidator<RegistrarFilaNotaContableDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public int mesesdiferencia = 0;
        public int diasdiferencia = 0;

        public Tercero TerceroAN8;
        public Tercero TerceroLM;
        public  CuentaContable CuentaContable;
        public Usuario Usuario;
        public ConfiguracionProcesoNotasContables ConfiguracionProceso  ;
        public NotaContable NotaContable;
        public RegistrarFilaNotaContableDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {     

            RuleFor(bdd => bdd.UsuarioId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El id del usuario creador es obligatorio.")
                .Must(ExisteUsuario).WithMessage("El usuario suministrado no fue encontrado en el sistema.");

            RuleFor(bdd => bdd.TerceroId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El id del tercero es obligatorio.")
                .Must(ExisteTerceroAN8).WithMessage("El tercero suministrado no fue encontrado en el sistema.");

            RuleFor(bdd => bdd.CuentaContableId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("La cuenta contable es obligatoria.")
                .Must(ExisteCuenta).WithMessage("La cuenta contable suministrada no fue encontrada en el sistema.");

            When(e => NotaContable != null, () =>
            {
                When(e => NotaContable.RevisionesGestionContable, () =>
                {
                    RuleFor(bdd => bdd.Fecha).Cascade(CascadeMode.StopOnFirstFailure)
                     .Must(validarFechaCierre).WithMessage("La fecha suministrada pertenece a un mes que ya fue cerrado.")
                     .Must(validarFecha).WithMessage("La fecha suministrada no puede tener mas de 3 meses de diferencia a la fecha actual.");

                    RuleFor(bdd => bdd.TerceroLMId).Cascade(CascadeMode.StopOnFirstFailure)
                     .NotEmpty().WithMessage("El codigo del LM es obligatorio.")
                     .Must(ExisteTerceroLM).WithMessage("El tercero suministrado en el campo LM no fue encontrado en el sistema.");
                });
                When(e => NotaContable.RevisionesFinanciacion && e.TerceroLMId != null, () =>
                {
                    RuleFor(bdd => bdd.TerceroLMId).Cascade(CascadeMode.StopOnFirstFailure)
                     .Must(ExisteTerceroLM).WithMessage("El tercero suministrado en el campo LM no fue encontrado en el sistema.");
                });
            });

            RuleFor(bdd => bdd.Importe).Cascade(CascadeMode.StopOnFirstFailure)
                 .NotEmpty().WithMessage("El valor del importe es obligatorio.");

            When(e => NotaContable != null && Usuario != null, () =>
            {
                RuleFor(a => new { request = a }).Custom((IdUsuarioEditor, context) =>
                {
                    NotaContable.PuedeEditar(Usuario).ToValidationFailure(context);
                });
            });
        }
        public bool ValidarImporte(long importe)
        {
            if (importe == 0) return false; else return true;
        }
        public bool validarFecha(DateTime? fecha)
        {
            mesesdiferencia = 0;
            diasdiferencia = 0;
            DateTime fechaverificada = fecha ?? DateTime.MinValue;
            if (fecha != null)
            {
                 mesesdiferencia = fechaverificada.Subtract(DateTime.Now).Days / 30;
                if (mesesdiferencia > 3 || mesesdiferencia < -3) { return false; } else return true;
            }
            else { return true; }
        }
        public bool validarFechaCierre(DateTime? fecha)
        {
            DateTime fechaverificada = fecha ?? DateTime.MinValue;
            if(fecha != null)
            {
                ConfiguracionProceso = _unitOfWork.GenericRepository<ConfiguracionProcesoNotasContables>()
                    .FindFirstOrDefault(e => e.Mes == fechaverificada.Month && e.Año == fechaverificada.Year && e.ProcesoId==NotaContable.ProcesoId);
                if (ConfiguracionProceso == null) {  return true; } else  return false;
            } else { return true; }
        }
        private bool ExisteUsuario(Guid id)
        {
            Usuario = _unitOfWork.UsuarioRepository.FindFirstOrDefault(e => e.Id == id);
            return Usuario != null;
        }
        private bool ExisteTerceroLM(Guid? id)
        {
            if (id != null) {
                TerceroLM = _unitOfWork.GenericRepository<Tercero>().FindFirstOrDefault(e => e.Id == id); return TerceroLM != null;
            } else return true;
        }
        private bool ExisteTerceroAN8(Guid id)
        {
            TerceroAN8 = _unitOfWork.GenericRepository<Tercero>().FindFirstOrDefault(e => e.Id == id);
            return TerceroAN8 != null;
        }
        private bool ExisteCuenta(Guid id)
        {
            CuentaContable = _unitOfWork.GenericRepository<CuentaContable>().FindFirstOrDefault(e => e.Id == id);
            return CuentaContable != null;
        }
    } 
}
