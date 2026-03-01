using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fsm_api.Models
{ 

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

     

    public class LeadListItem
    {
        public int LeadId { get; set; }
        public string LeadNumber { get; set; }
        public string LeadStatus { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string MobileNo { get; set; } = string.Empty;
        public DateTime? ScheduledOn { get; set; }

        // Concatenated or detailed address for the list view
        public string FullAddress { get; set; } = string.Empty;
    }

    public class LeadListViewModel
    {
        public int LeadId { get; set; }
        public string LeadNumber { get; set; }
        public DateTime LeadDate { get; set; }
        public DateTime? ScheduledOn { get; set; }
        public string CreatedBy { get; set; } // Maps to u.FullName
        public DateTime CreatedAt { get; set; }

        // Status Information
        public string StatusName { get; set; } // From WorkflowStatus

        // Customer Information
        public string CustomerName { get; set; } = string.Empty;
        public string MobileNo { get; set; } = string.Empty;
        public string WhatsappNo { get; set; }

        // Address Details
        public string AddressType { get; set; }
        public string Area { get; set; }
        public string Pincode { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        // Helper property to show a combined address in the UI
        public string DisplayAddress => $"{Area} ({Pincode})".Trim(' ', '(', ')');
        public long JobId { get; set; }
        public string JobNumber { get; set; }
        public string ServiceName { get; set; }
    }

}