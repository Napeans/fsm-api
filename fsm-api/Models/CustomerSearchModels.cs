using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fsm_api.Models
{
    public class CustomerSearchResponse
    {
        public bool IsNewCustomer { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string MobileNo { get; set; }
        public string WhatsappNo { get; set; }
        public string EmailId { get; set; }
        public List<CustomerAddressModel> Addresses { get; set; }
    }

    public class CustomerAddressModel
    {
        public int CustomerAddressId { get; set; }
        public string AddressType { get; set; }
        public string AddressLine1 { get; set; }
        public string Area { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public bool IsDefault { get; set; }
    }
}