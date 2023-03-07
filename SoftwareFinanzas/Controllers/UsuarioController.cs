using System.Collections.Generic;
using Application.Servicios.Usuarios;
using Domain.Contracts;
using Domain.Entities;
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
        [HttpGet]
        public IEnumerable<Usuario> Get()
        {
            return _unitOfWork.UsuarioRepository.FindBy(includeProperties:"Rol");
        }
        [HttpPost]
        public IActionResult Post(RegistrarUsuarioDto usuarioDto)
        {
            return Ok(_mediater.Send(usuarioDto).Result);
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
