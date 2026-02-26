using fsm_api.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace fsm_api.Controllers
{
    [Authorize]
    [RoutePrefix("api/jobs")]
    public class JobsController : ApiController
    {
        private readonly JobRepository _dal;
        public JobsController()
        {
            _dal = new JobRepository();
        }


        [HttpGet]
        [Route("GetMyJobs")]
        public async Task<IHttpActionResult> GetMyJobs()
        {
            var result = await _dal.GetMyJobs();

            return Ok(result);
        }
    }
}
