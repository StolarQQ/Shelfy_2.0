using Autofac;
using System.Reflection;
using FluentValidation;

namespace Shelfy.Infrastructure.IoC.Modules
{
    public class ValidationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = typeof(ValidationModule)
                .GetTypeInfo()
                .Assembly;

            builder.RegisterAssemblyTypes(assembly)
                .Where(x => x.IsAssignableTo<IValidator>())
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}