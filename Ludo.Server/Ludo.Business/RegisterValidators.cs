using FluentValidation;
using Ludo.RequestValidator.Entities;
using Ludo.RequestValidator.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Ludo.Business
{
    public static class RegisterValidators
    {
        public static void FromAssembly(IServiceCollection services)
        {
            var businessAssembly = Assembly.GetExecutingAssembly();

            if (businessAssembly != null)
            {
                List<Type> validatorTypes = businessAssembly.GetTypes()
                    .Where(type => type.GetConstructors()
                       .Any(inter => inter.DeclaringType.BaseType.Name == typeof(AbstractValidator<>).Name))
                    .ToList();

                foreach (Type validatorImplementation in validatorTypes)
                {
                    Type[] validatorGenericArguments = validatorImplementation.BaseType.GetGenericArguments();

                    if (validatorGenericArguments.Length == 0)
                    {
                        //TODO: Add custom exception
                        throw new NotImplementedException("Class named validator should implement Abstract Validator");
                    }

                    Type validatorType = typeof(IValidator<>).MakeGenericType(validatorGenericArguments.First());

                    services.AddSingleton(validatorType, validatorImplementation);

                    Type requestPrePreProcessorType = typeof(IRequestPreProcessor<>).MakeGenericType(validatorGenericArguments.First());
                    Type requestPreProcessorImplementation = typeof(ValidationPreProcess<>).MakeGenericType(validatorGenericArguments.First());

                    services.AddSingleton(requestPrePreProcessorType, requestPreProcessorImplementation);
                }
            }
        }
    }
}