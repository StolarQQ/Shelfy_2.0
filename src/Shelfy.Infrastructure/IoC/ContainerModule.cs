using Autofac;
using Microsoft.Extensions.Configuration;
using Shelfy.Infrastructure.AutoMapper;
using Shelfy.Infrastructure.IoC.Modules;

namespace Shelfy.Infrastructure.IoC
{
    public class ContainerModule : Autofac.Module
    {
        private readonly IConfiguration _configuration;

        public ContainerModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(AutoMapperConfig.Initialize())
                .SingleInstance();
            builder.RegisterModule<RepositoryModule>();
            builder.RegisterModule(new MongoModule(_configuration));
            builder.RegisterModule<ServiceModule>();
            builder.RegisterModule<ValidationModule>();
        }
    }
}