using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fsm_api.Models
{
    public class EstimateItem
    {
        public int SNo { get; set; }
        public string ItemName { get; set; }
        public string HSN { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
        public decimal Price { get; set; }
        public decimal Amount => Quantity * Price;
    }
    public class EstimateModel
    {
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string GSTIN { get; set; }

        public string CustomerName { get; set; }
        public string CustomerGST { get; set; }

        public string EstimateNo { get; set; }
        public DateTime EstimateDate { get; set; }

        public List<EstimateItem> Items { get; set; }

        public string LogoBase64 { get; set; }
        public string QrBase64 { get; set; }
        public byte[] ClientLogo { get; set; }

        public string ClientUPI { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }

        public string QuotationNumber { get; set; }
        public decimal Discount { get; set; }

        public string TermsText { get; set; }
        public string TechnicianSummary { get; set; }

        public string BankName { get; set; }
        public string AccountNo { get; set; }
        public string IFSCCode { get; set; }
        public string AccountHolder { get; set; }
        public byte[] ClientSignature { get; set; }

        public string ClientSignatureBase64 { get; set; }
    }

}