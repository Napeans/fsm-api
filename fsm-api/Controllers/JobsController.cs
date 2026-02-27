using Dapper;
using fsm_api.Models;
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

        [HttpGet]
        [Route("GetItems")]
        public async Task<IHttpActionResult> GetItems()
        {
            var result = await _dal.GetItems();

            return Ok(result);
        }


        [HttpPost]
        [Route("CreateQuotation")]
        public async Task<IHttpActionResult> CreateQuotation(CreateQuotation createQuotation)
        {
            var result = await _dal.CreateQuotation(createQuotation);

            return Ok(result);
        }


        [HttpPost]
        [Route("UpdateSatus")]
        public async Task<IHttpActionResult> UpdateSatus(UpdateStatusModel updateStatusModel)
        {
            var result = await _dal.UpdateSatus(updateStatusModel);

            return Ok(result);
        }
    }
}
