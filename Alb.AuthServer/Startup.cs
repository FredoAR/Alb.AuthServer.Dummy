using Alb.AuthServer.Application.Contracts.Identity;
using Alb.AuthServer.Application.Identity;
using Alb.AuthServer.Domain.Interfaces.Identity;
using Alb.AuthServer.Domain.Models.Options;
using Alb.AuthServer.Domain.Shared;
using Alb.AuthServer.Infrastructure;
using Alb.AuthServer.Infrastructure.EntityFrameworkCore.Context;
using Alb.AuthServer.Infrastructure.Identity;
using Microsoft.OpenApi.Models;


namespace Alb.Identity
{
    public class Startup
    {
        public IConfiguration Configuration { get; }



        public Startup(IConfiguration config)
        {
            Configuration = config;

        }


        public void ConfigureServices(IServiceCollection services)
        {
            /* ::: Add services to the container. ::: */

            /* Configuración de servicios de la capa de Infrastructure */
            services.AddInfrastructureModule(Configuration);
            services.AddTransient<AuthServerDbContextSeed>();


            /* Servicios durante el flujo normal */

            services.AddTransient<IIdentityAppService, IdentityAppService>();
            services.AddScoped<IAuthIdentityManager, AuthIdentityIntegration>();


            /* Options*/
            services.Configure<JwtOptions>(Configuration.GetSection(ConfigSections.Jwt));


            ConfigureMapper(services);

            /* Configurar serializador json */
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                /* Configuración de serialziador para remover del json response las propiedades nulas */
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            /* Configuración de encabezados en controladores */
            //builder.Services.AddControllers(options =>
            //{
            //    options.RespectBrowserAcceptHeader = true;
            //    options.ReturnHttpNotAcceptable = true;
            //});

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            ConfigureSwagger(services);

            ConfigureCors(services);
        }


        /// <summary>
        /// Configuración de CORS.
        /// Si agrego la configuración de CORS aquí en la configuración de servicios ya no lo agregoen web.config
        /// para evitar sobreescribir las reglas y causar conflictos.
        /// </summary>        
        public void ConfigureCors(IServiceCollection services)
        {            
            string allowedHosts = Configuration["AllowedHosts"] ?? "*";
            string[] allowedHostsList = allowedHosts.Split(',', StringSplitOptions.RemoveEmptyEntries);
                       
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>              
                {
                    builder.WithOrigins(allowedHostsList)
                    .AllowAnyMethod()
                    .AllowAnyHeader();                    
                });
            });
        }


        public void ConfigureSwagger(IServiceCollection services)
        {
            string version = "v1";
            string scheme = "Bearer";
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(version, new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Auth Server", Version = version });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
                options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                /* Configuración de Swagger para activar la autorización y poder enviar el encabezado con el token "Bearer {token}" */
                options.AddSecurityDefinition(scheme, new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme = scheme,
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header
                });
                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = scheme
                            }
                        }, new string[] { }
                    }
                });
            });
        }


        public void ConfigureMapper(IServiceCollection services)
        {
            services.AddAutoMapper(config =>
            {
                config.AddProfile<AlbAutoMapper>();
            });
        }



        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // TODO: En un nivel maduro de la API remover la linea env.IsProduction() del if
            if (env.IsDevelopment() || env.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Alb Identity Dummy API");
                    /* Inidica que swagger se abrira en la raiz, para usar esta linea comente {"launchUrl": "swagger", } en Properties > launchSettings.json */
                    options.RoutePrefix = string.Empty;
                });

                app.UseDeveloperExceptionPage();
            }            

            app.UseHttpsRedirection();
            app.UseRouting();
            
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
