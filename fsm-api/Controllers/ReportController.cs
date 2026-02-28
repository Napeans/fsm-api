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
        public async Task<HttpResponseMessage> DownloadJobReport()
        {
            var clientData = await _dal.GetClientDetails();

            var clientFirstData = clientData.FirstOrDefault();

            string qrBase64 = CommonMentods.GenerateUpiQrBase64(
    upiId: clientFirstData.ClientUPI,
    payeeName: clientFirstData.ClientName,
    amount: 17700.00m,
    note: "Estimate HF0002526047"
);
            string logoBase64 = Convert.ToBase64String(clientFirstData.ClientLogo);

            string html = $@"
<!DOCTYPE html>
<html>
<head>
<meta charset='UTF-8'>
<title>Estimate</title>
<style>
    body {{
        font-family: Arial, sans-serif;
        margin: 20px;
        font-size: 13px;
        color: #333;
    }}
    .header {{
        display: flex;
        justify-content: space-between;
        align-items: center;
    }}
    .company-info {{ line-height: 1.4; }}
    .logo img {{ height: 60px; }}
    .title {{
        text-align: center;
        font-size: 18px;
        color: #6a5acd;
        margin: 15px 0;
        border-bottom: 2px solid #6a5acd;
        padding-bottom: 5px;
    }}
    .section {{
        display: flex;
        justify-content: space-between;
        margin-top: 10px;
    }}
    .section div {{ width: 48%; }}
    table {{
        width: 100%;
        border-collapse: collapse;
        margin-top: 15px;
    }}
    th {{
        background-color: #6a5acd;
        color: white;
        padding: 6px;
        text-align: left;
        font-size: 12px;
    }}
    td {{
        padding: 6px;
        border-bottom: 1px solid #ddd;
        font-size: 12px;
    }}
    .text-right {{ text-align: right; }}
    .total-table {{
        width: 300px;
        float: right;
        margin-top: 10px;
    }}
    .total-row {{
        background-color: #dcd0ff;
        font-weight: bold;
    }}
    .footer {{
        margin-top: 80px;
        display: flex;
        justify-content: space-between;
    }}
    .signature {{ text-align: right; }}
    .small-text {{ font-size: 11px; }}
</style>
</head>
<body>

<div class='header'>
    <div class='company-info'>
        <strong>Mr.Home Fix</strong><br>
        26,Bharathi Street, Valayalam, Tirupur<br>
        Phone: 9944252644<br>
        Email: mrhomefixservice@gmail.com<br>
        GSTIN: 33AEXPS0132P1ZB<br>
        State: 33-Tamil Nadu
    </div>
    <div class='logo'>
        <img src='data:image/png;base64,{logoBase64}' width='120'/>
    </div>
</div>

<div class='title'>Estimate</div>

<div class='section'>
    <div>
        <strong>Estimate For</strong><br><br>
        M/S SELF LIFE INSURANCE COMPANY LIMITED<br>
        GSTIN Number: 33AEXPS0132P1ZB<br>
        State: 33-Tamil Nadu
    </div>

    <div class='text-right'>
        <strong>Estimate Details</strong><br><br>
        Estimate No: HF0002526047<br>
        Date: 25-02-2026
    </div>
</div>

<table>
<tr>
    <th>#</th>
    <th>Item name</th>
    <th>HSN/SAC</th>
    <th>Quantity</th>
    <th>Unit</th>
    <th class='text-right'>Price / unit</th>
    <th class='text-right'>Amount</th>
</tr>

<tr>
    <td>1</td>
    <td>PCB Board Repair & Installation charges.</td>
    <td>998713</td>
    <td>2</td>
    <td>Nos</td>
    <td class='text-right'>₹ 6,450.00</td>
    <td class='text-right'>₹ 12,900.00</td>
</tr>

<tr>
    <td>2</td>
    <td>Indoor swing motor.</td>
    <td>8535</td>
    <td>1</td>
    <td>Nos</td>
    <td class='text-right'>₹ 1,200.00</td>
    <td class='text-right'>₹ 1,200.00</td>
</tr>

<tr>
    <td>3</td>
    <td>Cassette AC gas charging full.</td>
    <td>998713</td>
    <td>1</td>
    <td>Nos</td>
    <td class='text-right'>₹ 3,600.00</td>
    <td class='text-right'>₹ 3,600.00</td>
</tr>
</table>

<table class='total-table'>
<tr>
    <td>Sub Total</td>
    <td class='text-right'>₹ 17,700.00</td>
</tr>
<tr class='total-row'>
    <td>Total</td>
    <td class='text-right'>₹ 17,700.00</td>
</tr>
</table>

<div style='clear: both;'></div>

<p><strong>Estimate Amount In Words</strong><br>
Seventeen Thousand Seven Hundred Rupees only</p>

<p class='small-text'>
Terms And Conditions<br>
Thank you for doing business with us.
</p>

<div class='footer'>
    <div>
        <img src='data:image/png;base64,{qrBase64}' height='120'/><br>
        <strong>Pay To:</strong><br>
        Bank Name: Canara Bank, Tirupur<br>
        IFSC code: CNRB0006231<br>
        Account Number: 120002057380<br>
        Account Holder's Name: MR.HOME FIX
    </div>

    <div class='signature'>
        For: Mr.Home Fix<br><br><br>
        Authorized Signatory
    </div>
</div>

</body>
</html>
";
            GeneratePDFs.Convert(html, @"D:\Estimate.pdf");

            //GeneratePDFs.Generate(@"D:\Estimate.pdf");
            var data = await _dal.getJobMediaData(2);




         

        

            var beforeImage = data
                .Where(p => p.Flag == "B")
                .Select(p => p.MediaData)
                .Where(p => p != null && p.Length > 0)
                .ToList();

            var afterImage = data
                .Where(p => p.Flag == "A")
                .Select(p => p.MediaData)
                .Where(p => p != null && p.Length > 0)
                .ToList();




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
                BeforeImages = beforeImage,
                AfterImages = afterImage,
                CompanyLogo= clientData.FirstOrDefault().ClientLogo,
                CustomerSignature= data.Where(p => p.Flag == "S").FirstOrDefault().MediaData
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

            return response;   // 🔥 IMPORTANT
        }
    }
}
