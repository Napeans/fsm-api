using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fsm_api.Models
{
    public class LeadModel
    {
        public int LeadId { get; set; }
        public string CustomerName { get; set; }
        public string ServiceName { get; set; }
        public string LeadStatus { get; set; }
        public int? JobId { get; set; }
    }
}