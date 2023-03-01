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
            var tercero = Validator.Tercero;
            var nuevoregistro = new Registrodenotacontable(usuario,tercero,cuenta,notacontable)
            {
               Fecha=request.Fecha,
               LM=request.LM,
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
        public Guid NotaContableId { get; set; }
        public long Importe { get; set; }
        public string? LM { get; set; }
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

        public Tercero Tercero;
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
            RuleFor(bdu => bdu.NotaContableId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("La nota contable es obligatoria para el registro.")
                .Must(ExisteNota).WithMessage($"La nota contable suministrada no fue encontrada en el sistema.");

            RuleFor(bdd => bdd.UsuarioId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El id del usuario creador es obligatorio.")
                .Must(ExisteUsuario).WithMessage("El usuario suministrado no fue encontrado en el sistema.");

            RuleFor(bdd => bdd.TerceroId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El id del tercero es obligatorio.")
                .Must(ExisteTercero).WithMessage("El tercero suministrado no fue encontrado en el sistema.");

            RuleFor(bdd => bdd.CuentaContableId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("La cuenta contable es obligatoria.")
                .Must(ExisteCuenta).WithMessage("La cuenta contable suministrada nof ue encontrada en el sistema.");

            RuleFor(bdd => bdd.Fecha).Cascade(CascadeMode.StopOnFirstFailure)
                .Must(validarFecha).WithMessage("La fecha suministrada no es aceptada.Recuerde que las fechas no deben pertener a un mes que ya ha sido cerrado," +
                " o fechas con diferencia mayor a 3 meses a la fecha actual.");

            RuleFor(bdd => bdd.Importe).Cascade(CascadeMode.StopOnFirstFailure)
                 .NotEmpty().WithMessage("El valor del importe es obligatorio.")
                 .Must(ValidarImporte).WithMessage("El valor del importe debe ser diferente de 0.");

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
            bool valido = true;
            mesesdiferencia = 0;
            diasdiferencia = 0;
            DateTime fechaverificada = fecha ?? DateTime.MinValue;
            if(fecha != null)
            {
                if (ConfiguracionProceso != null)
                {
                    mesesdiferencia = fechaverificada.Subtract(DateTime.Now).Days / 30;
                    if (mesesdiferencia > 3) valido = false;
                }
                else
                {

                }
                if (fechaverificada.Month == ConfiguracionProceso.Mes && fechaverificada.Year == ConfiguracionProceso.Año) valido = false;
            }
            return valido;
        }
        private bool ExisteUsuario(Guid id)
        {
            Usuario = _unitOfWork.UsuarioRepository.FindFirstOrDefault(e => e.Id == id);
            return Usuario != null;
        }
        private bool ExisteNota(Guid id)
        {
            NotaContable = _unitOfWork.GenericRepository<NotaContable>().FindFirstOrDefault(e => e.Id == id);
            if(NotaContable != null) ConfiguracionProceso = _unitOfWork.GenericRepository<ConfiguracionProcesoNotasContables>()
                    .FindBy(e => e.ProcesoId == NotaContable.ProcesoId).OrderByDescending(r => r.FechaCierre).FirstOrDefault();
            return NotaContable != null;
        }
        private bool ExisteTercero(Guid id)
        {
            Tercero = _unitOfWork.GenericRepository<Tercero>().FindFirstOrDefault(e => e.Id == id);
            return Tercero != null;
        }
        private bool ExisteCuenta(Guid id)
        {
            CuentaContable = _unitOfWork.GenericRepository<CuentaContable>().FindFirstOrDefault(e => e.Id == id);
            return CuentaContable != null;
        }
    }
}
