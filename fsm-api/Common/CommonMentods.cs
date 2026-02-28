using QRCoder;
using System;
using System.Security.Claims;
using System.Web;

namespace fsm_api.Common
{

    public static class CommonMentods
    {

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