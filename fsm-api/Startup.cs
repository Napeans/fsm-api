using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security.Jwt;
using Owin;
using System.Text;


[assembly: OwinStartup(typeof(fsm_api.Startup))]
namespace fsm_api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var key = Encoding.UTF8.GetBytes("THIS_IS_A_VERY_SECURE_SECRET_KEY");

            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            {
                AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Active,
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = "ScrapApp",
                    ValidAudience = "ScrapAppUsers",
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }
            });
        }
    }
}