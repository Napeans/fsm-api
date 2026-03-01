using fsm_api.Common;
using fsm_api.Models;
using fsm_api.Repository;
using iText.Html2pdf;
using System;
using System.Collections.Generic;
using System.IO;
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
        [Route("DownloadJobReport/{JobId:int}/{IsEstimate:bool}")]
        public async Task<HttpResponseMessage> DownloadJobReport(int JobId, bool IsEstimate)
        {
            var (estimate, items) = await _dal.GetInvoiceData(1, true);
            estimate.Items = items;

            decimal subTotal = items.Sum(x => x.Amount);
            string qrBase64 = CommonMentods.GenerateUpiQrBase64(
upiId: estimate.ClientUPI,
payeeName: estimate.CompanyName,
amount: subTotal,
note: (IsEstimate?"Estimate ":"Invoice ") + estimate.QuotationNumber
);


          
            estimate.QrBase64 = qrBase64;
            estimate.LogoBase64 = Convert.ToBase64String(estimate.ClientLogo);
            estimate.ClientSignatureBase64 = Convert.ToBase64String(estimate.ClientSignature);
            string html =(IsEstimate) ?CommonMentods.BuildTaxEstimateHtml(estimate): CommonMentods.BuildTaxInvoiceHtml(estimate);

            byte[] pdfBytes;

            using (MemoryStream ms = new MemoryStream())
            {
                ConverterProperties prop = new ConverterProperties();
                HtmlConverter.ConvertToPdf(html, ms, prop);
                pdfBytes = ms.ToArray();
            }

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(pdfBytes)
            };

            result.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/pdf");

            result.Content.Headers.ContentDisposition =
                new ContentDispositionHeaderValue("inline")
                {
                    FileName = "Estimate.pdf"
                };

            return result;
        }

      
    }
}
