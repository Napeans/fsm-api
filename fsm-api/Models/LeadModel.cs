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


    public class LeadCreateRequest
    {
        // Core Lead Details
        public int ServiceTypeId { get; set; }
        public DateTime ScheduledOn { get; set; }
        public string Remarks { get; set; }

        // Customer Identification
        public int CustomerId { get; set; } // 0 for New, > 0 for Returning
        public string CustomerName { get; set; }
        public string CustomerGST { get; set; }
        public string MobileNo { get; set; }
        public string WhatsappNo { get; set; }
        public string EmailId { get; set; }

        // Address Handling
        public int? CustomerAddressId { get; set; } // Used in Scenario 2
        public AddressRequest Addresse { get; set; } = null;// Used in Scenarios 1 & 3
      
    }

    public class AddressRequest
    {
        public int CustomerAddressId { get; set; } // Usually 0 for new addresses
        public string AddressType { get; set; }
        public string AddressLine1 { get; set; }
        public string Area { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string GoogleMapLink { get; set; }
        public bool IsDefault { get; set; }
    }


    public class LeadPaginationRequest
    {
        // Pagination Params
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        // Search Params
        public string SearchTerm { get; set; } // Search by name or mobile
        public int StatusId { get; set; }     
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public class LeadListResponse
    {
        // Pagination Metadata
        public int TotalCount { get; set; }
        public List<LeadListItem> Leads { get; set; } = new List<LeadListItem>();
    }

    public class LeadListItem
    {
        public int LeadId { get; set; }
        public string LeadStatus { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string MobileNo { get; set; } = string.Empty;
        public DateTime? ScheduledOn { get; set; }

        // Concatenated or detailed address for the list view
        public string FullAddress { get; set; } = string.Empty;
    }

}