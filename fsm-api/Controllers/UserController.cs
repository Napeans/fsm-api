using fsm_api.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace fsm_api.Controllers
{
    [Authorize]
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {

        private readonly UserRepository _dal;
        public UserController()
        {
            _dal = new UserRepository();
        }
        [HttpGet]
        [Route("GetCity")]
        public async Task<IHttpActionResult> GetCity()
        {
            var identity = (ClaimsIdentity)User.Identity;

            var userId = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var data = await _dal.GetCity();
            return Ok(data);
        }
        [HttpGet]
        [Route("GetSettings")]
        public async Task<IHttpActionResult> GetSettings(string key)
        {
            var data = await _dal.getSettings(key);
            return Ok(data);
        }





    }
}