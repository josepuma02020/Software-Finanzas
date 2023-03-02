using Application.Servicios.Usuarios;
using Domain.Contracts;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Configuration.ConfigUsuarios
{
    public class NotaContableController
    {
        private readonly IMediator _mediater;
        private readonly IUnitOfWork _unitOfWork;
        public NotaContableController(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediater = mediator;
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IEnumerable<Usuario> Get()
        {
            return _unitOfWork.NotaContableRepository.FindBy(includeProperties: "Rol");
        }
        [HttpPost]
        public IActionResult Post(RegistrarUsuarioDto usuarioDto)
        {
            return Ok(_mediater.Send(usuarioDto).Result);
        }
        [HttpPut("editarusuario")]
        public IActionResult ModificarRole(ModificarRoleDeUsuarioDto modificarRoleDeUsuario)
        {
            return Ok(_mediater.Send(modificarRoleDeUsuario).Result);
        }
        [HttpPut("editarusuario")]
        public IActionResult AsignarProceso(AsignarProcesoUsuarioDto modificarRoleDeUsuario)
        {
            return Ok(_mediater.Send(modificarRoleDeUsuario).Result);
        }
    }
}
