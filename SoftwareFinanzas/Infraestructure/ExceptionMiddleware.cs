using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace WebApi.Infrastructure
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ValidationException ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
            //catch (DomainException ex)
            //{
            //    await HandleExceptionAsync(httpContext, ex);
            //}
            catch (Exception ex)
            {
                if (ex.InnerException is ValidationException)
                {
                    await HandleExceptionAsync(httpContext, ex.InnerException as ValidationException);
                }
                else
                {
                    await HandleExceptionAsync(httpContext, ex);
                }

            }
        }

        private static Task HandleExceptionAsync(HttpContext context, ValidationException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            var errors = exception.Errors;

            return context.Response.WriteAsync(new ErrorValidation()
            {
                StatusCode = context.Response.StatusCode,
                Message = "Se han presentado algunos errores de validación," +
                " por favor revíse los datos y vuelva a realizar la operación.",
                Errors = errors
            }.ToString());
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var errors = new List<ValidationFailure>();
            errors.Add(new ValidationFailure("Servidor", exception.Message));
            return context.Response.WriteAsync(new ErrorValidation()
            {
                StatusCode = context.Response.StatusCode,
                Message = "Se ha presentado un error inesperado. Se ha enviado un correo" +
                " electrónico a Soporte para su pronta solución.",
                StackTrace = exception.StackTrace,
                Errors = errors

            }.ToString());
        }

        //private static Task HandleExceptionAsync(HttpContext context, DomainException exception)
        //{
        //    context.Response.ContentType = "application/json";
        //    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        //    var errors = new List<ValidationFailure>();
        //    errors.Add(new ValidationFailure("Aplicacion", exception.Message));
        //    return context.Response.WriteAsync(new ErrorValidation()
        //    {
        //        StatusCode = context.Response.StatusCode,
        //        Message = "Se han presentado algunos errores de validación de reglas de negocio, por favor revíse los datos y vuelva a realizar la operación.",
        //        StackTrace = exception.StackTrace,
        //        Errors = exception.Errors
        //    }.ToString());
        //}


    }
}
