using System.Collections.Generic;
using System.Threading.Tasks;
using Application;
using Application.Servicios.Usuarios;
using Domain.Contracts;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SoftwareFinanzas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IMediator _mediater;
        private readonly IUnitOfWork _unitOfWork;
        public UsuarioController(IMediator mediator,IUnitOfWork unitOfWork)
        {
            _mediater = mediator;
            _unitOfWork = unitOfWork;
        }
        [HttpPost]
        public ActionResult<IEnumerable<ConsultaUsuarioDto>> Post(GetUsuariosParametrizadaRequest parametros)
        {
            return Ok(_unitOfWork.UsuarioRepository.GetUsuarioParametrizados(parametros));
        }
        [HttpPost("Agregar")]
        public ActionResult Post(RegistrarUsuarioDto usuarioDto)
        {
            return Ok(_mediater.Send(usuarioDto));
        }
        [HttpPut("editarusuario/rol")]
        public IActionResult ModificarRole(ModificarRoleDeUsuarioDto modificarRoleDeUsuario)
        {
            return Ok(_mediater.Send(modificarRoleDeUsuario).Result);
        }
        [HttpPut("editarusuario/proceso")]
        public IActionResult AsignarProceso(AsignarProcesoUsuarioDto modificarRoleDeUsuario)
        {
            return Ok(_mediater.Send(modificarRoleDeUsuario).Result);
        }
    }
}
