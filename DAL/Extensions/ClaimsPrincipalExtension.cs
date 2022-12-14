using System.Security.Claims;

namespace DatingApp.DAL.Extensions
{
    public static class ClaimsPrincipalExtension
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirst("userId")?.Value;
        }
    }
}
