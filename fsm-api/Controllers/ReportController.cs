using fsm_api.Common;
using fsm_api.Models;
using fsm_api.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace fsm_api.Controllers
{
    //[Authorize]
    [RoutePrefix("api/report")]
    public class ReportController : ApiController
    {
        private readonly JobRepository _dal;
        public ReportController()
        {
            _dal = new JobRepository();
        }
        [HttpGet]
        [Route("DownloadJobReport")]
        public async Task<bool> DownloadJobReport()
        {
            var (estimate, items) = await _dal.GetInvoiceData(1, true);
            estimate.Items = items;

            decimal subTotal = items.Sum(x => x.Amount);
            string qrBase64 = CommonMentods.GenerateUpiQrBase64(
upiId: estimate.ClientUPI,
payeeName: estimate.CompanyName,
amount: subTotal,
note: "Estimate "+ estimate.QuotationNumber
);


            estimate.QrBase64 = qrBase64;
            estimate.LogoBase64 = Convert.ToBase64String(estimate.ClientLogo);
            string html = CommonMentods.BuildHtml(estimate);
            GeneratePDFs.Convert(html, @"D:\Estimate.pdf");


          
            return true;   // 🔥 IMPORTANT
        }
    }
}
