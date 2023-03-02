using Domain.Aplicacion;
using Domain.Contracts;
using Domain.Documentos.ConfiguracionDocumentos;
using Domain.Documentos;
using Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Servicios.Bases;
using System.Runtime.CompilerServices;
using Domain.Aplicacion.EntidadesConfiguracion;
using Application.Servicios.NotasContables.FilasdeNotaContable;
using Domain.Aplicacion.Entidades.CuentasContables;

namespace Application.Servicios.NotasContables
{
    public class EditarNotaContableCommand : IRequestHandler<EditarNotaContableDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public EditarNotaContableDtoValidator Validator { get; }

        public EditarNotaContableCommand(IUnitOfWork unitOfWork, IValidator<EditarNotaContableDto> validator)
        {
            _unitOfWork = unitOfWork;
            Validator = validator as EditarNotaContableDtoValidator;
        }
        public Task<Response> Handle(EditarNotaContableDto request, CancellationToken cancellationToken)
        {
            var notacontable = Validator.NotaContable;
            #region registrarFilasdeNotaContable
            var ListaRegistros = new List<Registrodenotacontable>();
            if (request.Tiponotacontable == Tiponotacontable.Concepto && request.Tiponotacontable == Tiponotacontable.registrosnota)
            {
                foreach (var item in request.FilasdeNotaContable)
                {
                    Tercero tercerolm = default;
                    var terceroAn8 = _unitOfWork.GenericRepository<Tercero>().FindFirstOrDefault(e => e.Id == item.TerceroId);
                    if (item.TerceroLMId != null) { tercerolm = _unitOfWork.GenericRepository<Tercero>().FindFirstOrDefault(e => e.Id == item.TerceroLMId); }
                    var cuentaContable = _unitOfWork.GenericRepository<CuentaContable>().FindFirstOrDefault(e => e.Id == item.CuentaContableId);
                    var usuario = _unitOfWork.UsuarioRepository.FindFirstOrDefault(e => e.Id == item.UsuarioId);

                    var nuevoregistro = new Registrodenotacontable(usuario, terceroAn8, tercerolm, cuentaContable, notacontable)
                    {
                        Fecha = item.Fecha,
                        Importe = item.Importe,
                        Id = Guid.NewGuid(),
                    };  
                    ListaRegistros.Add(nuevoregistro);
                }
                notacontable.SetRegistrosNotaContable(ListaRegistros);
            }
            #endregion
            
            _unitOfWork.GenericRepository<NotaContable>().Edit(notacontable.EditarNotaContable(notacontable));

            
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Mensaje = $"La nota contable  se editó correctamente."
            });
        }
    }
    public class EditarNotaContableDto : IRequest<Response>
    {
        public string? Comentario { get; set; }
        public DateTime? FechaNota { get; set; }
        public long Importe { get; set; }
        public List<RegistrarFilaNotaContableDto>? FilasdeNotaContable { get; set; }
        public Guid ClasificacionDocumentoId { get; set; }
        public Tiponotacontable Tiponotacontable { get; set; }
        public Guid TipoDocumentoId { get; set; }
        public Guid ProcesoId { get; set; }
        public Guid IdUsuarioEditor{ get; set; }
        public Guid NotaContableId { get; set; }
        public EditarNotaContableDto()
        {
        }
    }
    public class EditarNotaContableDtoValidator : AbstractValidator<EditarNotaContableDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClasificacionDocumento ClasificacionDocumento;
        public NotaContable NotaContable;
        public Usuario UsuarioEditor;
        public Proceso Proceso;
        public TipoDocumento TipoDocumento;
        public bool revisable;
        Configuracion Configuracion;
        public EditarNotaContableDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(e => e.NotaContableId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage($"La nota contable a editar es obligatoria.")
                .Must(ExisteNota).WithMessage($"La nota contable suministrada no fue encontrada en el sistema.");

            RuleFor(bdd => bdd.IdUsuarioEditor).Cascade(CascadeMode.StopOnFirstFailure)
               .NotEmpty().WithMessage("El id del usuario creador es obligatorio.")
               .Must(ExisteUsuario).WithMessage("El usuario suministrado no fue encontrado en el sistema.");

            When(a => NotaContable != null && UsuarioEditor != null, () =>
            {
                RuleFor(a => new { request = a }).Custom((IdUsuarioRechazador, context) =>
                {
                    NotaContable.PuedeEditar(UsuarioEditor).ToValidationFailure(context);
                });
            });
           
            RuleFor(bdu => bdu.ClasificacionDocumentoId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("La clasificación de la nota contable es obligatoria.")
                .Must(ExisteClasificacionDocumento).WithMessage($"La clasificación suministrada no fue encontrada en el sistema.");

            When(a => a.Tiponotacontable != null, () => {
                When(a => a.Tiponotacontable == Tiponotacontable.Soportes, () =>
                {
                    RuleFor(e => e.Importe).Cascade(CascadeMode.StopOnFirstFailure)
                   .NotEmpty().WithMessage("El valor del importe es obligatorio.")
                   .GreaterThanOrEqualTo(1000).WithMessage("El valor del importe debe ser mayor a 1000.")
                   .Must(Revisable).WithMessage("No se encontro configuracion de para comparar importe.");
                });
            });

            RuleFor(e => e.TipoDocumentoId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El tipo de documento es obligatorio.")
                .Must(ExsiteTipoDocumento).WithMessage($"No se encontro el tipo de documento en el sistema.");

            RuleFor(e => e.ProcesoId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Debe seleccionar el proceso al que se enviara la nota contable.")
                .Must(ExisteProceso).WithMessage("El proceso especificado no fue encontrado en el sistema.");

        }
        private bool ExisteNota(Guid id)
        {
            NotaContable = _unitOfWork.GenericRepository<NotaContable>().FindFirstOrDefault(e => e.Id == id);
            return NotaContable != null;
        }
        private bool Revisable(long importe)
        {
            bool valido = false;
            Configuracion = _unitOfWork.GenericRepository<Configuracion>().FindFirstOrDefault(t => t.Año == DateTime.Now.Year);
            if (Configuracion != null)
            {
                valido = true;
                var valorrevision = Configuracion.MultiploRevisarNotaContable * Configuracion.Salariominimo;
                if (importe >= valorrevision) { revisable = true; } else { revisable = false; }
            }
            else { revisable = false; valido = false; }
            return valido;
        }

        private bool ExisteUsuario(Guid id)
        {
            UsuarioEditor = _unitOfWork.UsuarioRepository.FindFirstOrDefault(e=> e.Id==id);
            return UsuarioEditor != null;
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
