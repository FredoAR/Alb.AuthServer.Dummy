
using Alb.AuthServer.Infrastructure.EntityFrameworkCore.Context;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Alb.Identity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = CreateHostBuilder(args);
            var host = builder.Build();

            var servicesProvider = host.Services;
            var logger = servicesProvider.GetRequiredService<ILogger<Program>>();

            try
            {
                using (var scope = servicesProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AuthServerDbContext>();
                    var seedCtx = scope.ServiceProvider.GetRequiredService<AuthServerDbContextSeed>();

                    var isAny = seedCtx.ContainsAnyUsers(dbContext).ConfigureAwait(false).GetAwaiter().GetResult();
                    
                    if (isAny is false)
                    {                        
                        seedCtx.SeedIdentityInfoBaseAsync(dbContext, scope.ServiceProvider).ConfigureAwait(false).GetAwaiter().GetResult();
                        logger.LogInformation($"{nameof(AuthServerDbContextSeed.SeedIdentityInfoBaseAsync)}. Ok");
                    }
                    else
                    {
                        logger.LogWarning($"{nameof(AuthServerDbContextSeed.SeedIdentityInfoBaseAsync)}. Users y Roles ya contienen registros.");
                    }
                }
            }
            catch (Exception ex)
            {                
                logger.LogError(ex, $"{nameof(AuthServerDbContextSeed.SeedIdentityInfoBaseAsync)}. Error al intentar ejecutar insertar datos semilla.");
                throw;
            }

            host.Run();
        }


        
        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(builder =>
                {
                    /* Configuración de startup */
                    builder.UseStartup<Startup>();

                    builder.ConfigureAppConfiguration(config =>
                    {
                        /* Archivos de configuracion, utilizo variables de entorno para determinar el archivo a utilizar */
                        config.AddJsonFile("appsettings.json");
                        config.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true);
                    });
                })
                /* Configuración de Serilog como logger */
                .UseSerilog((context, services, config) => {
                    config
                        .ReadFrom.Configuration(context.Configuration)
                        .ReadFrom.Services(services)
                        .Enrich.FromLogContext()
                        .Enrich.WithProperty("Application", typeof(Program).Assembly.GetName().Name)
                        .Enrich.WithProperty("Environment", context.HostingEnvironment)
                        .WriteTo.Console(theme: AnsiConsoleTheme.Code);
                });                
        }
    }
}