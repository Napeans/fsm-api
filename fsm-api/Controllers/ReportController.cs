using fsm_api.Common;
using fsm_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace fsm_api.Controllers
{
    //[Authorize]
    [RoutePrefix("api/report")]
    public class ReportController : ApiController
    {
        [HttpGet]
        [Route("DownloadJobReport")]
        public HttpResponseMessage DownloadJobReport()
        {
            var model = new JobReportModel
            {
                CompanyName = "APPLOGIQ",
                Technician = "ANAND PRASANTH S",
                Status = "Completed",
                Location = "Tiruppur Tamil Nadu 641601",
                JobNumber = "JOB 00329",
                BusinessUnit = "General",
                ServiceType = "AMC",
                BookedTime = "Mar-25-2025 | 22:21",
                ScheduledTime = "Mar-22-2025 | 09:30",
                BookedBy = "ANAND PRASANTH S",
                Asset = "-NA-",
                SerialNumber = "-NA-",
                ModelNumber = "-NA-",
                JobStartedTime = "Mar-25-2025 | 22:26",
                JobCompletedTime = "Mar-25-2025 | 22:32",
                TotalJobTime = "00:06:02",
                NoOfCrew = 1,
                JobTitle = "AMC - APPLOGIQ",
                ScopeOfWork = "SPLIT AC DEEP CLEAN-7 NOS & GAS TOPUP - 3 NOS",
                Notes = "PATCH THE HOLE WITH WHITE CEMENT",
                TechnicianNotes = "ALL AC FOAM JET SERVICE DONE...",
                BeforeImageUrls = new System.Collections.Generic.List<string>
            {
                "https://via.placeholder.com/300"
            },
                AfterImageUrls = new System.Collections.Generic.List<string>
            {
                "https://via.placeholder.com/300"
            }
            };

            var service = new JobReportPdfService();
            var pdfBytes = service.Generate(model);

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new ByteArrayContent(pdfBytes);
            response.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/pdf");

            response.Content.Headers.ContentDisposition =
                new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "JobReport.pdf"
                };

            return response;

        }
    }
}
