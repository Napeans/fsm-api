using fsm_api.Helper;
using fsm_api.Models;
using fsm_api.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace fsm_api.Controllers
{
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {
        private readonly UserRepository _dal;
        Mst_Users user = new Mst_Users() { UserId = 1, MobileNumber = 9698323236 };
        public AuthController()
        {
            _dal = new UserRepository();
        }
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Login(LoginModel model)
        {
            // 1️⃣ Validate input
            if (model == null || string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
                return BadRequest("Invalid login request");

            if (model.Username!="9698323236" ||   model.Password != "4321") {
                return BadRequest("Invalid login request");
            }


            // 2️⃣ Authenticate user (your DB logic)
            if (user == null)
                return Unauthorized();

            // 3️⃣ Generate tokens
            var accessToken = JwtTokenHelper.GenerateToken(user.UserId, user.MobileNumber.ToString());
            var refreshToken = RefreshTokenHelper.Generate();

            // 4️⃣ Capture device & request metadata
            var deviceId = model.DeviceId;

            // 5️⃣ Save refresh token (device-bound)
            await _dal.SaveRefreshToken(
                 user.UserId,
                 refreshToken,
                 deviceId
             );

            // 6️⃣ Return tokens
            return Ok(new
            {
                accessToken,
                refreshToken
            });
        }



        [HttpPost]
        [Route("refresh")]
        public async Task<IHttpActionResult> RefreshAsync(RefreshTokenRequest request)
        {
            var tokenData = await _dal.GetRefreshToken(request.RefreshToken);
            var token = tokenData.Where(x =>
                 x.Token == request.RefreshToken &&
                 !x.IsRevoked &&
                 x.ExpiryDate > DateTime.Now).FirstOrDefault();


            if (token == null)
                return Unauthorized();

            // 🔐 DEVICE ID CHECK
            if (!string.Equals(token.DeviceId, request.DeviceId, StringComparison.Ordinal))
                return Unauthorized();

            // 🔐 IP ADDRESS CHECK
            var currentIp = HttpContext.Current.Request.UserHostAddress;
            if (!string.Equals(token.IpAddress, currentIp, StringComparison.Ordinal))
                return Unauthorized();

            // 🔐 USER AGENT CHECK
            var currentUserAgent = HttpContext.Current.Request.UserAgent;
            if (!string.Equals(token.UserAgent, currentUserAgent, StringComparison.Ordinal))
                return Unauthorized();


            if (user == null)
                return Unauthorized();

            // 🔁 ROTATE TOKEN (VERY IMPORTANT)
            token.IsRevoked = true;

            var newRefreshToken = RefreshTokenHelper.Generate();
            await _dal.SaveRefreshToken(user.UserId, newRefreshToken, request.DeviceId);

            var newAccessToken = JwtTokenHelper.GenerateToken(user.UserId, user.MobileNumber.ToString());


            return Ok(new
            {
                accessToken = newAccessToken,
                refreshToken = newRefreshToken
            });
        }



        [HttpPost]
        [Authorize] // Optional but recommended
        [Route("auth/logout")]
        public async Task<IHttpActionResult> LogoutAsync(LogoutRequest request)
        {
            if (request == null ||
                string.IsNullOrEmpty(request.RefreshToken) ||
                string.IsNullOrEmpty(request.DeviceId))
            {
                return BadRequest("Invalid logout request");
            }
            var tokenData = await _dal.GetRefreshToken(request.RefreshToken);
            var token = tokenData.Where(x =>
                x.Token == request.RefreshToken &&
                x.DeviceId == request.DeviceId &&
                !x.IsRevoked).FirstOrDefault();


            if (token == null)
                return Ok(); // Already logged out (idempotent)

            // 🔐 Revoke refresh token
            _dal.RevokedToken(request.RefreshToken);

            return Ok(new { message = "Logged out successfully" });
        }

    }



    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string DeviceId { get; set; }
    }

    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; }
        public string DeviceId { get; set; }
    }
}
