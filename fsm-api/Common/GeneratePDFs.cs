using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Borders;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Html2pdf;

namespace fsm_api.Common
{
    public class GeneratePDFs
    {

        public static void Convert(string html, string outputPath)
        {
            using (FileStream fs = new FileStream(outputPath, FileMode.Create))
            {
                ConverterProperties prop = new ConverterProperties();
                HtmlConverter.ConvertToPdf(html, fs, prop);
            }
        }
        public static void Generate(string filePath)
        {
            PdfWriter writer = new PdfWriter(filePath);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            PdfFont normal = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

            // ===== Header =====
            Table headerTable = new Table(2).UseAllAvailableWidth();

            headerTable.AddCell(new Cell().Add(new Paragraph("Mr.Home Fix")
                .SetFont(bold).SetFontSize(16)).SetBorder(Border.NO_BORDER));

            headerTable.AddCell(new Cell().Add(new Paragraph("Estimate")
                .SetFont(bold).SetFontSize(16)
                .SetTextAlignment(TextAlignment.RIGHT))
                .SetBorder(Border.NO_BORDER));

            document.Add(headerTable);

            document.Add(new Paragraph("26,Bharathi Street, Valayalam, Tirupur")
                .SetFont(normal).SetFontSize(9));
            document.Add(new Paragraph("Phone: 9944252644")
                .SetFont(normal).SetFontSize(9));

            document.Add(new Paragraph("\n"));

            // ===== Estimate Info Section =====
            Table infoTable = new Table(2).UseAllAvailableWidth();

            infoTable.AddCell(new Cell().Add(new Paragraph("Estimate For")
                .SetFont(bold)).SetBorder(Border.NO_BORDER));

            infoTable.AddCell(new Cell().Add(new Paragraph("Estimate Details")
                .SetFont(bold).SetTextAlignment(TextAlignment.RIGHT))
                .SetBorder(Border.NO_BORDER));

            infoTable.AddCell(new Cell().Add(new Paragraph("M/S SELF LIFE INSURANCE COMPANY LIMITED\nGSTIN: 33AEXPS0132P1ZB\nState: Tamil Nadu"))
                .SetBorder(Border.NO_BORDER));

            infoTable.AddCell(new Cell().Add(new Paragraph("Estimate No: HF0002526047\nDate: 25-02-2026")
                .SetTextAlignment(TextAlignment.RIGHT))
                .SetBorder(Border.NO_BORDER));

            document.Add(infoTable);

            document.Add(new Paragraph("\n"));

            // ===== Items Table =====
            float[] columnWidths = { 1, 4, 2, 2, 2, 2 };
            Table table = new Table(columnWidths).UseAllAvailableWidth();

            string[] headers = { "#", "Item name", "HSN/SAC", "Qty", "Unit", "Amount" };
            foreach (var h in headers)
            {
                table.AddHeaderCell(new Cell().Add(new Paragraph(h).SetFont(bold))
                    .SetBackgroundColor(ColorConstants.LIGHT_GRAY));
            }

            // Row 1
            table.AddCell("1");
            table.AddCell("PCB Board Repair & Installation charges");
            table.AddCell("998713");
            table.AddCell("2");
            table.AddCell("Nos");
            table.AddCell("₹ 12,900.00");

            // Row 2
            table.AddCell("2");
            table.AddCell("Indoor swing motor");
            table.AddCell("8535");
            table.AddCell("1");
            table.AddCell("Nos");
            table.AddCell("₹ 1,200.00");

            // Row 3
            table.AddCell("3");
            table.AddCell("Cassette AC gas charging full");
            table.AddCell("998713");
            table.AddCell("1");
            table.AddCell("Nos");
            table.AddCell("₹ 3,600.00");

            document.Add(table);

            document.Add(new Paragraph("\n"));

            // ===== Total Section =====
            Table totalTable = new Table(2).SetWidth(200).SetHorizontalAlignment(HorizontalAlignment.RIGHT);

            totalTable.AddCell(new Cell().Add(new Paragraph("Sub Total"))
                .SetBorder(Border.NO_BORDER));
            totalTable.AddCell(new Cell().Add(new Paragraph("₹ 17,700.00"))
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetBorder(Border.NO_BORDER));

            totalTable.AddCell(new Cell().Add(new Paragraph("Total").SetFont(bold))
                .SetBackgroundColor(new DeviceRgb(230, 230, 250)));
            totalTable.AddCell(new Cell().Add(new Paragraph("₹ 17,700.00").SetFont(bold))
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetBackgroundColor(new DeviceRgb(230, 230, 250)));

            document.Add(totalTable);

            document.Add(new Paragraph("\n"));

            document.Add(new Paragraph("Seventeen Thousand Seven Hundred Rupees only")
                .SetFontSize(10));

            document.Add(new Paragraph("\nThank you for doing business with us."));

            document.Close();
        }
    }
}