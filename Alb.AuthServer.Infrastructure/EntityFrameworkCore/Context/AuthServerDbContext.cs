using Alb.AuthServer.Infrastructure.EntityFrameworkCore.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;


namespace Alb.AuthServer.Infrastructure.EntityFrameworkCore.Context
{


    /* Definición antes de integrar Identity */
    // public class AuthServerDbContext : DbContext

    /* Definición con contexto de Identity usando la clase original de IdentityUser */
    //public class AuthServerDbContext : IdentityDbContext<IdentityUser>

    /* Definición con contexto de Identity usando la clase extendida AuthIdentityUser para personalizar IdentityUser */
    public class AuthServerDbContext : IdentityDbContext<AuthIdentityUser>
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        public AuthServerDbContext(DbContextOptions<AuthServerDbContext> options) : base(options)
        {

        }

        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // TODO: investigar esta línea.
            //builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());           

            base.OnModelCreating(builder);

            /* Configuración de tabla */
            builder.Entity<AuthIdentityUser>(t =>
            {
                t.Property(p => p.UserName).IsRequired();
                t.Property(p => p.Email).IsRequired();
                t.Property(p => p.PasswordHash).IsRequired();
                t.Property(p => p.PhoneNumber).HasMaxLength(10);

                t.Property(p => p.RegistrationSource).HasColumnType("int").IsRequired();
                t.Property(p => p.RegistrationSourceDisplay).HasColumnType("varchar").HasMaxLength(64).IsRequired();
                t.Property(p => p.IsInternalClient).HasColumnType("bit");
            });
        }


    }
}
