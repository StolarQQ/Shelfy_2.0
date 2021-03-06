﻿using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using Shelfy.API.Framework.Extensions;
using Shelfy.Infrastructure.IoC;
using Shelfy.Infrastructure.Mongodb;

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
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    corsPolicyBuilder =>
                    {
                        corsPolicyBuilder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });

            services.AddControllers(x => x.InputFormatters
                .Insert(0, JsonFormatterExtension.GetJsonPatchInputFormatter())).AddNewtonsoftJson();
          

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Shelfy API", Version = "v1" });
            });

            // JWT configuration
            services.RegisterJwt(Configuration);
            services.AddMemoryCache();
            services.AuthorizationExtension();

            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterModule(new ContainerModule(Configuration));
            ApplicationContainer = builder.Build();

            return new AutofacServiceProvider(ApplicationContainer);
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            // Serilog configuration
            loggerFactory.AddSerilog();
            app.RegisterSerilog();

            app.UseRouting();
            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
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
            app.UseAuthorization();

        
            app.UseHttpsRedirection();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
