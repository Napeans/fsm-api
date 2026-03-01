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
        public static string BuildTaxInvoiceHtml(EstimateModel model)
        {
            StringBuilder rows = new StringBuilder();
            int i = 1;

            foreach (var item in model.Items)
            {
                rows.Append($@"
<tr>
    <td>{i++}</td>
    <td style='text-align:center'>{item.ItemName}</td>
    <td style='text-align:center'>{item.HSN}</td>
    <td style='text-align:center'>{item.Quantity}</td>
    <td style='text-align:center'>{item.Unit}</td>
    <td style='text-align:center'>₹ {item.Price:N2}</td>
    <td style='text-align:center'>₹ {item.Amount:N2}</td>
</tr>");
            }

            decimal subTotal = model.Items.Sum(x => x.Amount);
            decimal Quantity = model.Items.Sum(x => x.Quantity);
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

@page {{
    size: A4;
    margin: 15mm;
}}

body {{
    font-family: 'Segoe UI', Arial, sans-serif;
    font-size:13px;
    color:#222;
}}

.header {{
    display:flex;
    justify-content:space-between;
    border-bottom:3px solid #1f4e79;
    padding-bottom:12px;
}}

.title {{
    text-align:center;
    font-size:22px;
    font-weight:700;
    margin:18px 0;
    color:#1f4e79;
}}

table {{
    width:100%;
    border-collapse:collapse;
}}

th {{
    background:#1f4e79;
    color:white;
    padding:8px;
}}

td {{
    padding:6px;
    border:1px solid #ddd;
    text-align:center;
}}

.items-table td:nth-child(2) {{
    text-align:left;
}}

.summary-section {{
    margin-top:15px;
    display:flex;
    justify-content:space-between;
}}

.left-box {{
    width:55%;
    font-size:12px;
    line-height:1.6;
}}

.right-box {{
    width:40%;
}}

.total-table td {{
    border:none;
    padding:4px;
}}

.total-table {{
    width:100%;
}}

.grand-total {{
    font-weight:bold;
    background:#f2f2f2;
}}

.qr-bank-section {{
    margin-top:30px;
    display:flex;
    justify-content:space-between;
    align-items:flex-end;
}}

.qr-left {{
    width:48%;
}}

.bank-box {{
    border:1px solid #ddd;
    padding:8px;
    margin-top:8px;
    font-size:12px;
    line-height:1.5;
}}

.signature-right {{
    width:48%;
    text-align:right;
}}

.footer {{
    margin-top:25px;
    border-top:1px solid #ccc;
    padding-top:8px;
    font-size:12px;
    display:flex;
    justify-content:space-between;
}}

</style>
</head>

<body>

<!-- HEADER -->
<div class='header'>
    <div>
        <strong>{model.CompanyName}</strong><br>
        {model.CompanyAddress}<br>
        Phone: {model.Phone}<br>
        Email: {model.Email}<br>
        GSTIN: {model.GSTIN}
    </div>

    <div>
        <img src='data:image/png;base64,{model.LogoBase64}' 
             style='width:180px;height:70px;'/>
    </div>
</div>

<div class='title'>TAX INVOICE</div>

<!-- CUSTOMER -->
<table style='margin-bottom:10px;'>
<tr>
<td style='border:none;text-align:left;'>
<strong>Bill To:</strong><br>
{model.CustomerName}<br>
GSTIN: {model.CustomerGST}
</td>

<td style='border:none;text-align:right;'>
<strong>Invoice No:</strong> {model.EstimateNo}<br>
<strong>Date:</strong> {model.EstimateDate:dd-MM-yyyy}
</td>
</tr>
</table>

<!-- ITEMS -->
<table class='items-table'>
<tr>
<th>#</th>
<th>Description</th>
<th>HSN</th>
<th>Qty</th>
<th>Unit</th>
<th>Rate</th>
<th>Amount</th>
</tr>

{rows}
<tr style='background: #A4C8E8;font-weight: bold;'><td></td>
            <td style='text-align:center;font-weight:bold'>Total</td><td></td>
             <td style='text-align:center'>{Quantity}</td><td></td><td style='text-align:right'></td>
               <td style='text-align:center'>₹ {subTotal:N2}</td></tr>
</table>

<!-- DETAILS + TOTAL -->
<div class='summary-section'>

<div class='left-box'>

<strong>Description:</strong><br>
{model.TechnicianSummary}<br><br>

<strong>Terms & Conditions:</strong><br>
{model.TermsText}<br><br>

<strong>Invoice Amount In Words:</strong><br>
{ConvertAmount(grandTotal)}

</div>

<div class='right-box'>

<table class='total-table'>
<tr>
<td>Sub Total</td>
<td style='text-align:right;'>₹ {subTotal:N2}</td>
</tr>
<tr>
<td>Discount</td>
<td style='text-align:right;'>- ₹ {discount:N2}</td>
</tr>
<tr>
<td>Taxable Value</td>
<td style='text-align:right;'>₹ {taxableAmount:N2}</td>
</tr>
<tr>
<td>SGST @9%</td>
<td style='text-align:right;'>₹ {sgst:N2}</td>
</tr>
<tr>
<td>CGST @9%</td>
<td style='text-align:right;'>₹ {cgst:N2}</td>
</tr>
<tr class='grand-total'>
<td>Grand Total</td>
<td style='text-align:right;'>₹ {grandTotal:N2}</td>
</tr>
</table>

</div>

</div>

<!-- QR + SIGNATURE SAME ROW -->
<div class='qr-bank-section'>

<!-- LEFT: QR + BANK -->
<div class='qr-left'>

<strong>Scan to Pay</strong><br><br>
<table><tbody><tr><td>
   <img src='data:image/png;base64,{model.QrBase64}' 
     width='130' />
</td><td>
    
        <div>

<strong>Pay To:</strong><br>
Bank Name:{model.BankName}<br>
Account No: {model.AccountNo}<br>
IFSC Code: {model.IFSCCode}<br>
Account Holder: {model.AccountHolder}

</div>
</td></tr></tbody></table>

</div>

<!-- RIGHT: SIGNATURE -->
<div class='signature-right'>

<strong>For {model.CompanyName}</strong>

<br>
  <img src='data:image/png;base64,{model.ClientSignatureBase64}' 
             style='width:180px;height:70px;'/>
<br>

<strong>Authorized Signatory</strong>

</div>

</div>

<!-- FOOTER -->
<div class='footer'>
<div>GST Invoice - Computer Generated</div>
<div>Thank you for doing business with us</div>
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
        private static string[] unitsMap =
    {
        "Zero", "One", "Two", "Three", "Four", "Five", "Six",
        "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve",
        "Thirteen", "Fourteen", "Fifteen", "Sixteen",
        "Seventeen", "Eighteen", "Nineteen"
    };

        private static string[] tensMap =
        {
        "Zero", "Ten", "Twenty", "Thirty", "Forty",
        "Fifty", "Sixty", "Seventy", "Eighty", "Ninety"
    };

        public static string ConvertAmount(decimal amount)
        {
            if (amount == 0)
                return "Zero Rupees only";

            long intPart = (long)Math.Floor(amount);
            int decimalPart = (int)((amount - intPart) * 100);

            string words = ConvertNumber(intPart) + " Rupees";

            if (decimalPart > 0)
            {
                words += " and " + ConvertNumber(decimalPart) + " Paise";
            }

            return words + " only";
        }

        private static string ConvertNumber(long number)
        {
            if (number == 0)
                return "Zero";

            if (number < 0)
                return "Minus " + ConvertNumber(Math.Abs(number));

            StringBuilder words = new StringBuilder();

            if ((number / 1000000) > 0)
            {
                words.Append(ConvertNumber(number / 1000000) + " Million ");
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words.Append(ConvertNumber(number / 1000) + " Thousand ");
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words.Append(ConvertNumber(number / 100) + " Hundred ");
                number %= 100;
            }

            if (number > 0)
            {
                if (words.Length != 0)
                    words.Append("");

                if (number < 20)
                    words.Append(unitsMap[number]);
                else
                {
                    words.Append(tensMap[number / 10]);
                    if ((number % 10) > 0)
                        words.Append(" " + unitsMap[number % 10]);
                }
            }

            return words.ToString().Trim();
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