using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;
using Serilog;
using Shelfy.API.Framework.Extensions;
using Shelfy.Core.Domain;
using Shelfy.Core.Repositories;
using Shelfy.Infrastructure.AutoMapper;
using Shelfy.Infrastructure.Mongodb;
using Shelfy.Infrastructure.Repositories;
using Shelfy.Infrastructure.Services;
using Swashbuckle.AspNetCore.Swagger;

namespace Shelfy.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthorization(x =>
            {
                x.AddPolicy("HasUserRole", p => p.RequireRole(Role.User.ToString(), Role.Admin.ToString()));
                x.AddPolicy("HasModeratorRole", p => p.RequireRole(Role.Moderator.ToString(), Role.Admin.ToString()));
                x.AddPolicy("HasAdminRole", p => p.RequireRole(Role.Admin.ToString()));
            });

            services.AddSingleton<IMongoClient, MongoClient>(x =>
                new MongoClient(Configuration["ConnectionStrings:ShelfyDatabase"]));
            services.AddTransient<IMongoDatabase>(x =>
                x.GetRequiredService<IMongoClient>().GetDatabase(Configuration["Mongo:Database"]));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEncrypterService, EncrypterService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IDataSeeder, DataSeeder>();

            services.AddSingleton<IJwtHandler, JwtHandler>();
            services.AddSingleton(AutoMapperConfig.Initialize());


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Shelfy API", Version = "v1" });
            });

            // JWT configuration
            services.RegisterJwt(Configuration);
            services.AddMemoryCache();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(x => x.SerializerSettings.Formatting = Formatting.Indented);
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

            // Enable middleware to serve generated Swagger as a JSON endpoint.
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
