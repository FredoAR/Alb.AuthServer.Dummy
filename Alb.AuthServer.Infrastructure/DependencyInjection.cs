using Alb.AuthServer.Infrastructure.EntityFrameworkCore.Context;
using Alb.AuthServer.Infrastructure.EntityFrameworkCore.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace Alb.AuthServer.Infrastructure
{
    public static class DependencyInjection
    {                
        /// <summary>
        /// Registro de servicios de la capa de Infraestructura
        /// </summary>        
        public static IServiceCollection AddInfrastructureModule(this IServiceCollection services, IConfiguration configuration)
        {

            services.ConfigureDbContext(configuration);

            services.ConfigureIdentity(configuration);

            services.ConfigureJwt(configuration);        

            return services;
        }


        /// <summary>
        /// Método de extensión para configirar el Contexto de base de datos
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration">IConfiguration obtiene acceso a las configuraciones de appsettings.</param>
        private static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AuthServerDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    m => m.MigrationsAssembly("Alb.AuthServer.Infrastructure"));
            });

            /* Registro de repositorio generico, en caso de existir tambien se registran los repositorios especificos */
            //services.AddScoped<DbContext, AuthServerDbContext>();
            //services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }


        /// <summary>
        /// Configuración de Identity para autenticación y autorización de usuarios
        /// </summary>
        /// <param name="services"></param>
        private static void ConfigureIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<AuthIdentityUser, IdentityRole>(options =>            
            {
                // User settings.
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;

                // SignIn settings.
                //options.SignIn.RequireConfirmedEmail = false;
                //options.SignIn.RequireConfirmedPhoneNumber = false;

                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 10;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.AllowedForNewUsers = true;                
            })
            .AddEntityFrameworkStores<AuthServerDbContext>()
            .AddDefaultTokenProviders();    //añade proveedores de tokens predeterminados, como los que se utilizan para la recuperación de contraseñas, confirmación de correo electrónico, etc.

        }



        private static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor()   /* Registra el IHttpContextAccessor que nos permite acceder el HttpContextde cada solicitud */
            .AddAuthorization()                 /* Dependencias necesarias para autorizar solicitudes (como autorización por roles) */
            .AddAuthentication(options =>       /* Agrega esquema de autenticación a usar, por default autenticación por Bearer Tokens */
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>    /* Configura la autenticación por tokens, especificando que debe de validar y que llave privada utilizar */
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Jwt:ValidIssuer"],
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    //ValidAudience = configuration["Jwt:ValidAudience"],
                    // ValidAudiences = jwtSettings.GetSection("ValidAudiences").Get<List<string>>(),  // Configura múltiples audiencias
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
                };
            });
        }



    }
}
