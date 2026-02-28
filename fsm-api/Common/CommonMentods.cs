using System.Security.Claims;
using System.Web;

namespace fsm_api.Common
{

    public static class CommonMentods
    {


 
        public static int UserId
        {
            get
            {
                var context = HttpContext.Current;

                if (context == null || context.User == null)
                    return 0;

                var identity = context.User.Identity as ClaimsIdentity;

                if (identity == null)
                    return 0;

                var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

                int userId;
                return int.TryParse(claim?.Value, out userId) ? userId : 0;
            }
        }
    }
}