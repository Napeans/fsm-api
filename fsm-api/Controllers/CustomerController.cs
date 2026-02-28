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
    [RoutePrefix("api/customers")]
    public class CustomerController : ApiController
    {
        private readonly CustomerRepository _dal;
        public CustomerController()
        {
            _dal = new CustomerRepository();
        }


        [HttpGet]
        [Route("search")]
        public async Task<IHttpActionResult> Search(string mobile)
        {
            if (string.IsNullOrEmpty(mobile))
                return BadRequest("Mobile number required");

            var result = await _dal.SearchCustomer(mobile);

            return Ok(result);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IHttpActionResult> create(CustomerCreateRequest request)
        {
            
            return Ok("Customer Created");
        }
    }
}
