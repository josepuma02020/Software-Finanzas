using Application.Servicios.NotasContables.FilasdeNotaContable;
using Domain.Aplicacion;
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
            var clasificacion = Validator.ClasificacionDocumento;
            var nuevanotacontable = new NotaContable(Validator.UsuarioCreador)
            {
               revisable=Validator.revisable,
               EquipoId=Validator.UsuarioCreador.EquipoId,
               Comentario= request.Comentario,
               Importe=request.Importe,
               Tiponotacontable=request.Tiponotacontable,
               ClasificacionDocumento= clasificacion,
               Id = Guid.NewGuid(),
               Proceso=Validator.Proceso,
               FechaNota = request.FechaNota,
            };
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
        public string? Comentario { get; set; }
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

        public ClasificacionDocumento ClasificacionDocumento;

        public Proceso Proceso;
        public Equipo EquipoCreador;
        public TipoDocumento TipoDocumento;
        public Usuario UsuarioCreador;
        public bool revisable;
        Configuracion Configuracion;
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

            When(a => a.Tiponotacontable != null,()=>{
                When(a => a.Tiponotacontable == Tiponotacontable.Soportes, () =>
                {
                    RuleFor(e => e.Importe).Cascade(CascadeMode.StopOnFirstFailure)
                   .NotEmpty().WithMessage("El valor del importe es obligatorio.")
                   .GreaterThanOrEqualTo(1000).WithMessage("El valor del importe debe ser mayor a 1000.")
                   .Must(Revisable).WithMessage("No se encontro configuracion de para comparar importe.");
                });
                When(a => a.Tiponotacontable == Tiponotacontable.registrosnota, () =>
                {
                   
                });
            });

            RuleFor(e => e.TipoDocumentoId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El tipo de documento es obligatorio.")
                .Must(ExsiteTipoDocumento).WithMessage($"No se encontro el tipo de documento en el sistema.");

            RuleFor(e => e.ProcesoId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Debe seleccionar el proceso al que se enviara la nota contable.")
                .Must(ExisteProceso).WithMessage("El proceso especificado no fue encontrado en el sistema.");

        }
        private bool Revisable(long importe)
        {
            bool valido = false;
            Configuracion = _unitOfWork.GenericRepository<Configuracion>().FindFirstOrDefault(t => t.Año == DateTime.Now.Year);
            if(Configuracion != null) 
            {
                valido = true;
                var valorrevision = Configuracion.MultiploRevisarNotaContable * Configuracion.Salariominimo;
                if (importe >= valorrevision) { revisable = true; } else { revisable = false; }
            }
            else { revisable = false;valido = false; }
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
