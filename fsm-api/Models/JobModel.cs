using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fsm_api.Models
{
    public class JobsModel
    {
        public int JobId { get; set; }
        public string JobNumber { get; set; }
        public string CustomerName { get; set; }
        public string MobileNo { get; set; }
        public string ServiceName { get; set; }
        public DateTime? ScheduledOn { get; set; }
        public string Address { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string JobStatus { get; set; }
    }
}