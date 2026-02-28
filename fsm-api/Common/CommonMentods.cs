using fsm_api.Models;
using QRCoder;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace fsm_api.Common
{

    public static class CommonMentods
    {
        public static string BuildHtml(EstimateModel model)
        {
            StringBuilder rows = new StringBuilder();
            int i = 1;

            foreach (var item in model.Items)
            {
                rows.Append($@"
<tr>
    <td>{i++}</td>
    <td>{item.ItemName}</td>
    <td>{item.HSN}</td>
    <td>{item.Quantity}</td>
    <td>{item.Unit}</td>
    <td>₹ {item.Price:N2}</td>
    <td>₹ {item.Amount:N2}</td>
</tr>");
            }

            decimal subTotal = model.Items.Sum(x => x.Amount);

            decimal discount = model.Discount;
            decimal taxableAmount = subTotal - discount;

            decimal sgst = taxableAmount * 0.09m;
            decimal cgst = taxableAmount * 0.09m;

            decimal grandTotal = taxableAmount + sgst + cgst;

            return $@"
<!DOCTYPE html>
<html>
<head>
<meta charset='UTF-8'>
<style>
body {{ font-family: Arial; margin:20px; font-size:13px; }}
.title {{
    text-align:center;
    font-size:18px;
    color:#6a5acd;
    border-bottom:2px solid #6a5acd;
    padding-bottom:5px;
}}
table {{ width:100%; border-collapse:collapse; margin-top:15px; }}
th {{
    background:#6a5acd;
    color:white;
    padding:6px;
}}
td {{
    padding:6px;
    border:1px solid #ddd;
    text-align:center;
}}
.total-row {{
    background:#dcd0ff;
    font-weight:bold;
}}
.footer {{
    margin-top:80px;
    display:flex;
    justify-content:space-between;
}}
</style>
</head>
<body>

<div style='display:flex; justify-content:space-between;'>
<div>
<strong>{model.CompanyName}</strong><br>
{model.CompanyAddress}<br>
Phone: {model.Phone}<br>
Email: {model.Email}<br>
GSTIN: {model.GSTIN}
</div>
<div>
<img src='data:image/png;base64,{model.LogoBase64}' style='width:250px;height:70px'/>
</div>
</div>

<h2 class='title'>Estimate</h2>

<div style='display:flex; justify-content:space-between;'>
<div>
<strong>Estimate For</strong><br><br>
{model.CustomerName}<br>
GSTIN: {model.CustomerGST}
</div>
<div style='text-align:right;'>
<strong>Estimate Details</strong><br><br>
Estimate No: {model.EstimateNo}<br>
Date: {model.EstimateDate:dd-MM-yyyy}
</div>
</div>

<table>
<tr>
<th>#</th>
<th>Item</th>
<th>HSN</th>
<th>Qty</th>
<th>Unit</th>
<th>Price</th>
<th>Amount</th>
</tr>

{rows}
</table>

<table style='width:350px; float:right; margin-top:10px;'>
<tr>
    <td>Sub Total</td>
    <td>₹ {subTotal:N2}</td>
</tr>
<tr>
    <td>Discount</td>
    <td>- ₹ {discount:N2}</td>
</tr>
<tr>
    <td>Taxable Amount</td>
    <td>₹ {taxableAmount:N2}</td>
</tr>
<tr>
    <td>SGST @ 9%</td>
    <td>₹ {sgst:N2}</td>
</tr>
<tr>
    <td>CGST @ 9%</td>
    <td>₹ {cgst:N2}</td>
</tr>
<tr class='total-row'>
    <td>Grand Total</td>
    <td>₹ {grandTotal:N2}</td>
</tr>
</table>

<div style='clear:both;'></div>

<br/>
<strong>Payment Mode:</strong> Credit
<br/><br/>

<strong>Scan to Pay</strong><br/>
<img src='data:image/png;base64,{model.QrBase64}' width='150'/>

<div class='footer'>
<div>
Bank Name: Canara Bank<br>
Account Number: 120002057380
</div>
<div style='text-align:right;'>
For: {model.CompanyName}<br><br><br>
Authorized Signatory
</div>
</div>

</body>
</html>";
        }
        public static string GenerateUpiQrBase64(
            string upiId,
            string payeeName,
            decimal amount,
            string note)
        {
            // IMPORTANT: Properly encoded UPI string
            string upiUrl =
                "upi://pay" +
                "?pa=" + Uri.EscapeDataString(upiId) +
                "&pn=" + Uri.EscapeDataString(payeeName) +
                "&am=" + amount.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) +
                "&cu=INR" +
                "&tn=" + Uri.EscapeDataString(note);

            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                var qrData = qrGenerator.CreateQrCode(
                    upiUrl,
                    QRCodeGenerator.ECCLevel.H  // VERY IMPORTANT
                );

                using (PngByteQRCode qrCode = new PngByteQRCode(qrData))
                {
                    byte[] qrBytes = qrCode.GetGraphic(
                        pixelsPerModule: 25,  // Bigger = better scanning
                        darkColor: System.Drawing.Color.Black,
                        lightColor: System.Drawing.Color.White,
                        drawQuietZones: true
                    );

                    return Convert.ToBase64String(qrBytes);
                }
            }
        }

        public static int UserId
        {
            get
            {
                var context = HttpContext.Current;

                if (context == null || context.User == null)
                    return 0;

                var identity = context.User.Identity as ClaimsIdentity;

                if (identity == null)
                    return 0;

                var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

                int userId;
                return int.TryParse(claim?.Value, out userId) ? userId : 0;
            }
        }
    }
}