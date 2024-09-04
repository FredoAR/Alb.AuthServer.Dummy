
namespace Alb.AuthServer.Domain.Shared.Identity
{
    public static class IdentityConsts
    {
        public static class AdminUser
        {
            public const string Password = "alB-Pwd";

            public const string Email = "email@domain.com";
        }

        public static class UserRoles
        {
            public const string Admin = "admin";

            public const string User = "user";
        }

        public static class Claims
        {
            public const string EmailJwtClaim = "";
        }
    }
}
