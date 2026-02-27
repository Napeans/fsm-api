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

        public string CurrentButtion { get; set; }

        public DateTime? StartTime { get; set; }

        public int QuotationId { get; set; }
    }
    public class JobMediaModel {
        public int JobId { get; set; }
        public int MediaId { get; set; }
        public string[] MediaDatas { get; set; }

        public byte[] MediaData { get; set; }
        public string Flag { get; set; }
    }
    public class CreateQuotation
    {
        public int JobId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal DiscountValue { get; set; }

        public decimal CGST { get; set; }

        public decimal SGST { get; set; }
        public decimal TotalAmount { get; set; }

        public string Status { get; set; }

        public int CreatedBy { get; set; }
        public string Items { get; set; }
    }
    public class JobMediaResponseModel
    {
        public int MediaId { get; set; }
        public string Base64Image { get; set; }
        public string Flag { get; set; }
    }
    public class QuotationItemsModel{
        public int QuotationId { get; set; }
        public int QuotationItemId { get; set; }
        public int ItemId { get; set; }
        public decimal Quantity { get; set; }
        public string Flag { get; set; }

        

    }
    public class UpdateStatusModel
    {
        public int JobId { get; set; }
        public string Status { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

    }

    public class QuotationItem {
        public int ItemId { get; set; }

        public int QuotationItemId { get; set; }
        public string ItemName { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
    public class Items
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string HSNCode { get; set; }
        public string UOM { get; set; }
        public decimal DefaultPrice { get; set; }

        public bool IsActive { get; set; }
    }
}