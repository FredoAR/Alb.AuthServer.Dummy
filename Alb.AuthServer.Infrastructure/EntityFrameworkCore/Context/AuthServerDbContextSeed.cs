using Alb.AuthServer.Domain.Shared.Enums;
using Alb.AuthServer.Domain.Shared.Identity;
using Alb.AuthServer.Infrastructure.EntityFrameworkCore.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Alb.AuthServer.Infrastructure.EntityFrameworkCore.Context
{
    public class AuthServerDbContextSeed
    {

        private readonly UserManager<AuthIdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public AuthServerDbContextSeed(UserManager<AuthIdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public async Task<bool> ContainsAnyUsers(AuthServerDbContext context)
        {            
            return await context.Users.AnyAsync() && await context.UserRoles.AnyAsync();            
        }
        

        public async Task SeedIdentityInfoBaseAsync(AuthServerDbContext context, IServiceProvider serviceProvider)
        {
            if (!context.Users.Any())
            {
                /* Agregar roles */
                await _roleManager.CreateAsync(GetAdminRole());
                await _roleManager.CreateAsync(GetUserRole());

                /* Agregar AdminUser en dbo.AspNetUsers */
                var newAdminUser = GetFirtsAdminUser();
                await _userManager.CreateAsync(newAdminUser, IdentityConsts.AdminUser.Password);
                await _userManager.AddToRoleAsync(newAdminUser, IdentityConsts.UserRoles.Admin);
            }
        }



        private AuthIdentityUser GetFirtsAdminUser()
        {
            return new AuthIdentityUser
            {                
                UserName = IdentityConsts.AdminUser.Email,
                NormalizedUserName = IdentityConsts.AdminUser.Email.ToUpper(),
                Email = IdentityConsts.AdminUser.Email,
                NormalizedEmail = IdentityConsts.AdminUser.Email.ToUpper(),
                EmailConfirmed = true,
                //PasswordHash = Pass,
                SecurityStamp = null,
                ConcurrencyStamp = null,
                PhoneNumber = null,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnd = null,
                LockoutEnabled = false,
                AccessFailedCount = 0,

                IsInternalClient = true,
                RegistrationSource = RegistrationSources.Api,
                RegistrationSourceDisplay = nameof(RegistrationSources.Api)                
            };
        }


        private IdentityRole GetAdminRole() => new IdentityRole
        {
            Name = IdentityConsts.UserRoles.Admin,
            NormalizedName = IdentityConsts.UserRoles.Admin.ToUpper(),
        };


        private IdentityRole GetUserRole() => new IdentityRole
        {
            Name = IdentityConsts.UserRoles.User,
            NormalizedName = IdentityConsts.UserRoles.User.ToUpper(),
        };

    }
}
