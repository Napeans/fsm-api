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
    [RoutePrefix("api/leads")]
    public class LeadsController : ApiController
    {
        private readonly LeadRepository _repo;

        public LeadsController()
        {
            _repo = new LeadRepository();
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> Get()
        {
            var data = await _repo.GetLeads();
            return Ok(data);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            await _repo.DeleteLead(id);
            return Ok();
        }

        [HttpPost]
        [Route("{id}/convert")]
        public async Task<IHttpActionResult> Convert(int id)
        {
            await _repo.ConvertToJob(id);
            return Ok();
        }

        [HttpPost]
        [Route("create")]
        public async Task<IHttpActionResult> Createlead(LeadCreateRequest request)
        {

            var leadId = await _repo.CreateLead(request);
            return Ok(leadId);
        }
    }
}
