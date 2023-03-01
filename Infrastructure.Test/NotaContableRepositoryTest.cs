﻿using Domain.Aplicacion.EntidadesConfiguracion;
using Domain.Base;
using Domain.Documentos;
using Domain.Entities;
using Domain.Repositories;
using Infraestructure.Context;
using Infraestructure.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Test
{
    public class NotaContableRepositoryTest
    {

        private  FinanzasContext context;
        public static Guid IdGenericaabierta = Guid.NewGuid();
        public static Guid IdGenericaabierta2 = Guid.NewGuid();
        public static Guid IdGenericacerrada = Guid.NewGuid();
        public static Guid IdAreaFinanzas = Guid.NewGuid();
        public static Guid IdAreaProyectos = Guid.NewGuid();
        public static Guid IdUsuario1 = Guid.NewGuid();
        public static Guid IdUsuario2 = Guid.NewGuid();
        [SetUp]
        public void Setup()
        {
            var option = new DbContextOptionsBuilder<FinanzasContext>().
            UseInMemoryDatabase("NotaContableRepository").Options;
            context = new FinanzasContext(option);
            #region AgrearAreadePrueba
             Area areaFinanzas = new Area("Finanzas")
             {
             Id = IdAreaFinanzas,
             CodigoDependencia = "12345",
             };
             context.Areas.Add(areaFinanzas);
             context.SaveChanges();
            #endregion
            #region AgrearAreadePrueba
            Area areaProyectos = new Area("Proyectos")
            {
                Id = IdAreaProyectos,
                CodigoDependencia = "12345",
            };
            context.Areas.Add(areaProyectos);
            context.SaveChanges();
            #endregion
            #region AgregarUsuarioPrueba
            Usuario usuario1 = new Usuario()
            {
                Id = IdUsuario1,Nombre="Jose",
                Proceso = new Proceso("Flujo de caja", Guid.NewGuid()) { Area = areaProyectos }
            };
            context.Usuarios.Add(usuario1);
            context.SaveChanges();
            #endregion
            #region AgregarNotaContablePruebaAbierta
            NotaContable notacontableabierta = new NotaContable()
            {
                UsuarioCreador = new Usuario() { Id = IdUsuario2, Nombre ="Jose" , Proceso = new Proceso("Flujo de caja",Guid.NewGuid()) { Area = areaFinanzas } },
                ClasificacionDocumento = new ClasificacionDocumento() { Descripcion="Notas"},
                Anulador=usuario1,AnuladorId=IdUsuario1,
                
                TipoDocumento = new TipoDocumento(),Comentario = "Nota1",
                Importe = 1000000,
                Tiponotacontable = Tiponotacontable.registrosnota,
                Id = IdGenericaabierta,
                EstadoDocumento = EstadoDocumento.Abierto,Proceso = new Proceso("Financiacion",Guid.NewGuid()),
            };
            context.NotasContables.Add(notacontableabierta);
            context.SaveChanges();
            #endregion
            #region AgregarNotaContablePruebaAbierta
            NotaContable notacontableabierta2 = new NotaContable()
            {
                UsuarioCreador = usuario1,
                CreadorId = IdUsuario1,
                ClasificacionDocumento = new ClasificacionDocumento() { Descripcion = "Notas" },
                TipoDocumento = new TipoDocumento(),Comentario="Nota2",
                Importe = 1000000,
                Tiponotacontable = Tiponotacontable.registrosnota,
                Id = IdGenericaabierta2,
                Verificador = null,
                EstadoDocumento = EstadoDocumento.Abierto,
                Proceso = new Proceso("Financiacion", Guid.NewGuid()),
            };
            context.NotasContables.Add(notacontableabierta2);
            context.SaveChanges();
            #endregion
            #region AgregarNotaContablePruebacerrada
            NotaContable notacontablecerrada = new NotaContable()
            {
                UsuarioCreador = usuario1,
                CreadorId = IdUsuario1,
                ClasificacionDocumento = new ClasificacionDocumento() { Descripcion = "Notas" },
                TipoDocumento = new TipoDocumento(),Comentario="Nota3",
                Importe = 122222,
                Tiponotacontable = Tiponotacontable.registrosnota,
                Id = IdGenericacerrada,
                EstadoDocumento = EstadoDocumento.Cerrado,
                Proceso = new Proceso("Financiacion", Guid.NewGuid()),
            };
            context.NotasContables.Add(notacontablecerrada);
            context.SaveChanges();
        }
        #endregion
        [Test]
        public void NotasContablexEstado()
        {
            NotaContableRepository repository = new NotaContableRepository(context);
            GetNotasContablesParametrizadaRequest request = new GetNotasContablesParametrizadaRequest()
            {
                EstadoDocumento = EstadoDocumento.Abierto,
            };

            List<ConsultaNotasContablesDTO> resultado = repository.GetNotasContablesParametrizadas(request).ToList();
            foreach (var nota in resultado)
            {
                TestContext.WriteLine(nota.EstadoDocumento + nota.NombreUsuarioVerificador);
            }
        }
        [Test]
        public void NotasContablexArea()
        {
            NotaContableRepository repository = new NotaContableRepository(context);
            GetNotasContablesParametrizadaRequest request = new GetNotasContablesParametrizadaRequest()
            {
                FiltroArea=true,AreaId = IdAreaProyectos,
            };

            List<ConsultaNotasContablesDTO> resultado = repository.GetNotasContablesParametrizadas(request).ToList();
            foreach (var nota in resultado)
            {
                TestContext.WriteLine(nota.Comentario+"/"+nota.NombreAreaUsuarioCreador + "/" + nota.NombreEquipoUsuarioCreador+"/" + nota.NombreProcesoUsuarioCreador);
            }
        }
        [Test]
        public void NotaContablexUsuario()
        {
            NotaContableRepository repository = new NotaContableRepository(context);
            GetNotasContablesParametrizadaRequest request = new GetNotasContablesParametrizadaRequest()
            {
                IdUsuarioFiltro= IdUsuario1,
                TipoFiltroUsuario=TipoFiltroUsuario.All,
            };

            List<ConsultaNotasContablesDTO> resultado = repository.GetNotasContablesParametrizadas(request).ToList();
            foreach (var nota in resultado)
            {
                TestContext.WriteLine(nota.Comentario + "/" + nota.NombreAreaUsuarioCreador + "/" + nota.NombreEquipoUsuarioCreador + "/" + nota.NombreProcesoUsuarioCreador+"/"+nota.EstadoDocumento);
            }
        }
        [Test]
        public void NotaContablexComentario()
        {
            NotaContableRepository repository = new NotaContableRepository(context);
            GetNotasContablesParametrizadaRequest request = new GetNotasContablesParametrizadaRequest()
            {
                FiltroComentario=true,Comentario="Nota1",
            };

            List<ConsultaNotasContablesDTO> resultado = repository.GetNotasContablesParametrizadas(request).ToList();
            foreach (var nota in resultado)
            {
                TestContext.WriteLine(nota.Comentario + "/" + nota.NombreAreaUsuarioCreador + "/" + nota.NombreEquipoUsuarioCreador + "/" + nota.NombreProcesoUsuarioCreador + "/" + nota.EstadoDocumento);
            }
        }
    }
}
