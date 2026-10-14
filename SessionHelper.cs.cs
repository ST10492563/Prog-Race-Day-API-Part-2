using Microsoft.AspNetCore.Http;

namespace RaceDayApi.Security
{
    public static class SessionHelper
    {
        public static int? GetUserID(HttpContext httpContext)
        {
            return httpContext.Session.GetInt32("UserID");
        }

        public static string? GetRole(HttpContext httpContext)
        {
            return httpContext.Session.GetString("Role");
        }

        public static bool IsLoggedIn(HttpContext httpContext)
        {
            return GetUserID(httpContext).HasValue;
        }

        public static bool IsOrganiser(HttpContext httpContext)
        {
            return GetRole(httpContext) == "Organiser";
        }

        public static bool IsParticipant(HttpContext httpContext)
        {
            return GetRole(httpContext) == "Participant";
        }
    }
}
