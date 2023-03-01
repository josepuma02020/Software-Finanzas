using Domain.Base;
using FluentValidation;
using FluentValidation.Validators;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Application.Servicios.Bases
{
    public static class ValidationResponseConvert
    {
        public static void ToValidationFailure<T>(this DomainValidation validation, ValidationContext<T> context)
        {
            var failures = validation.Fallos.ToList();
            failures.ForEach(error =>
            {
                error.Value.ToList().ForEach(messageError => context.AddFailure(error.Key, messageError));
            });
        }
    }
}
