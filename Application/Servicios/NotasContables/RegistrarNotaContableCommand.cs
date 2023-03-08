using Application.Servicios.NotasContables.FilasdeNotaContable;
using Domain.Aplicacion;
using Domain.Aplicacion.Entidades.CuentasContables;
using Domain.Aplicacion.EntidadesConfiguracion;
using Domain.Contracts;
using Domain.Documentos;
using Domain.Documentos.ConfiguracionDocumentos;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Servicios.NotasContables
{
    public class RegistrarNotaContableCommand : IRequestHandler<RegistrarNotaContableDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegistrarNotaContableDtoValidator Validator { get; }

        public RegistrarNotaContableCommand(IUnitOfWork unitOfWork, IValidator<RegistrarNotaContableDto> validator)
        {
            _unitOfWork = unitOfWork;
            Validator = validator as RegistrarNotaContableDtoValidator;
        }
        public Task<Response> Handle(RegistrarNotaContableDto request, CancellationToken cancellationToken)
        {
            #region RegistrarNota
            var clasificacion = Validator.ClasificacionDocumento;
            var nuevanotacontable = new NotaContable(Validator.UsuarioCreador)
            {
                SalarioMinimoVigente = Validator.Configuracion.Salariominimo,
                ProcesoId = Validator.Proceso.Id,
                RevisionesFinanciacion = request.RevisionFinanciacion,
                RevisionesGestionContable = request.RevisionGestionContable,
                TipoDocumento = Validator.TipoDocumento,
                TipoDocumentoId = request.TipoDocumentoId,

                revisable = Validator.revisable,
                EquipoId = Validator.UsuarioCreador.EquipoId,
                Comentario = request.Comentario,
                Importe = request.Importe,
                Tiponotacontable = request.Tiponotacontable,
                ClasificacionDocumento = clasificacion,
                Id = Guid.NewGuid(),
                Proceso = Validator.Proceso,
                FechaNota = request.FechaNota,
            };
            #endregion

            #region registrarFilasdeNotaContable
            if(request.Tiponotacontable == Tiponotacontable.Concepto && request.Tiponotacontable == Tiponotacontable.registrosnota)
            {
                foreach (var item in request.FilasdeNotaContable)
                {
                    Tercero tercerolm = default;
                    var terceroAn8 = _unitOfWork.GenericRepository<Tercero>().FindFirstOrDefault(e => e.Id == item.TerceroId);
                    if (item.TerceroLMId != null) { tercerolm = _unitOfWork.GenericRepository<Tercero>().FindFirstOrDefault(e => e.Id == item.TerceroLMId); }
                    var cuentaContable = _unitOfWork.GenericRepository<CuentaContable>().FindFirstOrDefault(e => e.Id == item.CuentaContableId);
                    var usuario = _unitOfWork.UsuarioRepository.FindFirstOrDefault(e => e.Id == item.UsuarioId);

                    var nuevoregistro = new Registrodenotacontable(usuario, terceroAn8, tercerolm, cuentaContable, nuevanotacontable)
                    {
                        Fecha = item.Fecha,
                        Importe = item.Importe,
                        Id = Guid.NewGuid(),
                    };
                    nuevanotacontable.Registrosnota.Add(nuevoregistro);
                }
            }
            #endregion
            _unitOfWork.GenericRepository<NotaContable>().Add(nuevanotacontable);
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Data = nuevanotacontable,
                Mensaje = $"La nota contable se registró correctamente."
            });
        }
    }
    public class RegistrarNotaContableDto : IRequest<Response>
    {
        public bool RevisionFinanciacion { get; set; }
        public bool RevisionGestionContable { get; set; }
        public string? Comentario { get; set; }
        public List<RegistrarFilaNotaContableDto>? FilasdeNotaContable  { get; set; }
        public DateTime? FechaNota { get; set; }
        public long Importe { get; set; }
        public Guid ClasificacionDocumentoId { get; set; }
        public  Tiponotacontable  Tiponotacontable { get; set; }
        public  Guid TipoDocumentoId { get; set; }
        public Guid ProcesoId { get; set; }
        public Guid UsuarioCreadorId { get; set; }
        public List<RegistrarFilaNotaContableDto>? Registrodenota { get; set; }
        public List<AppFile>? Soportes { get; set; }
        public RegistrarNotaContableDto()
        {
        }
    }
    public class RegistrarNotaContableDtoValidator : AbstractValidator<RegistrarNotaContableDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public int mesesdiferencia = 0;
        public int diasdiferencia = 0;

        public ClasificacionDocumento ClasificacionDocumento;
        public ConfiguracionProcesoNotasContables ConfiguracionProceso;
        public Proceso Proceso;
        public Equipo EquipoCreador;
        public TipoDocumento TipoDocumento;
        public Usuario UsuarioCreador;
        public bool revisable;
        public Configuracion Configuracion;
        public RegistrarNotaContableDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(bdd => bdd.UsuarioCreadorId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El id del usuario creador es obligatorio.")
                .Must(ExisteUsuario).WithMessage("El usuario suministrado no fue encontrado en el sistema.");

            RuleFor(bdu => bdu.ClasificacionDocumentoId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("La clasificación de la nota contable es obligatoria.")
                .Must(ExisteClasificacionDocumento).WithMessage($"La clasificación suministrada no fue encontrada en el sistema.");
            
            RuleFor(e => e.ProcesoId).Cascade(CascadeMode.StopOnFirstFailure)
               .NotEmpty().WithMessage("Debe seleccionar el proceso al que se enviara la nota contable.")
               .Must(ExisteProceso).WithMessage("El proceso especificado no fue encontrado en el sistema.");

            RuleFor(e => e.TipoDocumentoId).Cascade(CascadeMode.StopOnFirstFailure)
              .NotEmpty().WithMessage("El tipo de documento es obligatorio.")
              .Must(ExsiteTipoDocumento).WithMessage($"No se encontro el tipo de documento en el sistema.");

            When(a => a.Tiponotacontable != null && Proceso != null,()=>{
                When(a => a.Tiponotacontable == Tiponotacontable.Soportes, () =>
                {
                    RuleFor(e => e.Importe).Cascade(CascadeMode.StopOnFirstFailure)
                   .NotEmpty().WithMessage("El valor del importe es obligatorio para notas contables de tipo soportes.")
                   .Must(Revisable).WithMessage("No se encontro configuracion de para comparar importe.");

                    RuleFor(bdd => bdd.FechaNota).Cascade(CascadeMode.StopOnFirstFailure)
                          .NotEmpty().WithMessage("La fecha de nota es obligatoria para soportes.");

                    When(e => e.RevisionGestionContable && e.FechaNota != null, () =>
                    {
                        RuleFor(bdd => bdd.FechaNota).Cascade(CascadeMode.StopOnFirstFailure)
                          .Must(validarFechaCierre).WithMessage("La fecha suministrada pertenece a un mes que ya fue cerrado.")
                          .Must(validarFecha).WithMessage("La fecha suministrada no puede tener mas de 3 meses de diferencia a la fecha actual.");
                    });
                });
                When(a => a.Tiponotacontable == Tiponotacontable.registrosnota, () =>
                {
                    RuleFor(e => e.FilasdeNotaContable).NotEmpty().WithMessage("Los registros son obligatorios.");

                    When(e => e.FilasdeNotaContable != null, () =>
                    {
                        RuleForEach(e => e.FilasdeNotaContable).SetValidator(new RegistrarFilaNotaContableDtoValidator(_unitOfWork));
                    });
                });
            });
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
            if (fecha != null)
            {
                ConfiguracionProceso = _unitOfWork.GenericRepository<ConfiguracionProcesoNotasContables>()
                    .FindFirstOrDefault(e => e.Mes == fechaverificada.Month && e.Año == fechaverificada.Year && e.ProcesoId == Proceso.Id);
                if (ConfiguracionProceso == null) { return true; } else return false;
            }
            else { return true; }
        }
        private bool Revisable(long importe)
        {
            bool valido = false;
            Console.WriteLine("Año actual:" + DateTime.Now.Year);
            Configuracion = _unitOfWork.GenericRepository<Configuracion>().FindFirstOrDefault(t => t.Año == DateTime.Now.Year);
            if(Configuracion != null) 
            {
                valido = true;
                var valorrevision = Configuracion.MultiploRevisarNotaContable * Configuracion.Salariominimo;
                if (importe >= valorrevision) { revisable = true; } else { revisable = false; }
            }
            return valido;
        }
        private bool ExisteUsuario(Guid id)
        {
            UsuarioCreador = _unitOfWork.UsuarioRepository.FindFirstOrDefault(e=>e.Id==id);
            return UsuarioCreador != null;
        }
        private bool ExisteProceso(Guid id)
        {
            Proceso = _unitOfWork.GenericRepository<Proceso>()
               .FindFirstOrDefault(e => e.Id == id);
            return Proceso != null;
        }
        private bool ExisteClasificacionDocumento(Guid id)
        {
             ClasificacionDocumento = _unitOfWork.GenericRepository<ClasificacionDocumento>()
                .FindFirstOrDefault(e => e.Id == id);
            return ClasificacionDocumento != null;
        }
        private bool ExsiteTipoDocumento(Guid id)
        {
            TipoDocumento = _unitOfWork.GenericRepository<TipoDocumento>()
               .FindFirstOrDefault(e => e.Id == id);
            return TipoDocumento != null;
        }
    }
}
