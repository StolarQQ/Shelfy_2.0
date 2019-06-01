using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using Shelfy.API.Framework.Extensions;
using Shelfy.Core.Domain;
using Shelfy.Infrastructure.IoC;
using Shelfy.Infrastructure.Mongodb;
using Swashbuckle.AspNetCore.Swagger;

namespace Shelfy.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IContainer ApplicationContainer { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddAuthorization(x =>
            {
                x.AddPolicy("HasUserRole", p => p.RequireRole(Role.User.ToString(), Role.Admin.ToString()));
                x.AddPolicy("HasModeratorRole", p => p.RequireRole(Role.Moderator.ToString(), Role.Admin.ToString()));
                x.AddPolicy("HasAdminRole", p => p.RequireRole(Role.Admin.ToString()));
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", cors =>
                    cors.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Shelfy API", Version = "v1" });
            });

            // JWT configuration
            services.RegisterJwt(Configuration);
            services.AddMemoryCache();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(x => x.SerializerSettings.Formatting = Formatting.Indented);

            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterModule(new ContainerModule(Configuration));
            ApplicationContainer = builder.Build();

            return new AutofacServiceProvider(ApplicationContainer);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // Serilog configuration
            loggerFactory.AddSerilog();
            app.RegisterSerilog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors("CorsPolicy");
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shelfy API V1");
            });

            // Register conventions for mongodb
            MongoConfiguration.Initialize();

            app.UseMyExceptionHandler();
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
