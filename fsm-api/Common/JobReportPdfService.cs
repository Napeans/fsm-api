using fsm_api.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;

namespace fsm_api.Common
{
    public class JobReportPdfService
    {
        public byte[] Generate(JobReportModel model)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(20);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Content().Layers(layers =>
                    {
                        // WATERMARK
                        layers.Layer()
                            .AlignCenter()
                            .AlignMiddle()
                            .Rotate(-45)
                            .Text("JOB REPORT")
                            .FontSize(80)
                            .Bold()
                            .FontColor(Colors.Grey.Lighten3);

                        layers.PrimaryLayer().Column(main =>
                        {
                            // ================= HEADER =================
                            main.Item().Container()
                                .Background(Colors.Grey.Lighten4)
                                .Padding(15)
                                .Row(row =>
                                {
                                    // LEFT SIDE
                                    row.RelativeItem().Column(col =>
                                    {
                                        col.Item().Text("Job Report")
                                            .FontSize(20)
                                            .Bold()
                                            .FontColor(Colors.Blue.Darken2);

                                        col.Item().PaddingTop(5)
                                            .Text(model.JobNumber ?? "-")
                                            .FontSize(12)
                                            .SemiBold();

                                        col.Item().PaddingTop(3)
                                            .Text(model.BookedTime ?? "-")
                                            .FontSize(9)
                                            .FontColor(Colors.Grey.Darken1);

                                        // STATUS BADGE (INLINE SAFE)
                                        col.Item().PaddingTop(6).Container()
                                            .Background(
                                                (model.Status ?? "").ToLower() == "completed"
                                                    ? Colors.Green.Medium
                                                    : (model.Status ?? "").ToLower() == "in progress"
                                                        ? Colors.Orange.Medium
                                                        : Colors.Red.Medium
                                            )
                                            .PaddingVertical(4)
                                            .PaddingHorizontal(10)
                                            .AlignLeft()
                                            .Text(model.Status ?? "-")
                                            .FontColor(Colors.White)
                                            .FontSize(9)
                                            .Bold();
                                    });

                                    // RIGHT SIDE LOGO
                                    if (model.CompanyLogo != null)
                                    {
                                        row.ConstantItem(100)
                                            .AlignRight()
                                            .Height(60)
                                            .Image(model.CompanyLogo, ImageScaling.FitArea);
                                    }
                                });

                            main.Item().PaddingVertical(12);

                            // ================= CONTACT + QUICK INFO =================
                            main.Item().Row(row =>
                            {
                                row.RelativeItem().Container()
                                    .Background(Colors.Grey.Lighten5)
                                    .Padding(12)
                                    .Column(c =>
                                    {
                                        c.Item().Text("Contact Information")
                                            .Bold()
                                            .FontSize(12)
                                            .FontColor(Colors.Blue.Darken2);

                                        InfoLine(c, "Company", model.CompanyName);
                                        InfoLine(c, "Technician", model.Technician);
                                        InfoLine(c, "Location", model.Location);
                                    });

                                row.RelativeItem().Container()
                                    .Background(Colors.Grey.Lighten5)
                                    .Padding(12)
                                    .Column(c =>
                                    {
                                        c.Item().Text("Quick Info")
                                            .Bold()
                                            .FontSize(12)
                                            .FontColor(Colors.Blue.Darken2);

                                        c.Item().PaddingTop(8).Grid(grid =>
                                        {
                                            grid.Columns(2);

                                            AddDetailBox(grid, "Service Type", model.ServiceType);
                                            AddDetailBox(grid, "Business Unit", model.BusinessUnit);
                                            AddDetailBox(grid, "Crew", model.NoOfCrew.ToString());
                                            AddDetailBox(grid, "Total Time", model.TotalJobTime);
                                        });
                                    });
                            });

                            main.Item().PaddingVertical(10);

                            // ================= JOB DETAILS =================
                            main.Item().Container()
                                .Background(Colors.Grey.Lighten5)
                                .Padding(12)
                                .Column(c =>
                                {
                                    c.Item().Text("Job Details")
                                        .Bold()
                                        .FontSize(12)
                                        .FontColor(Colors.Blue.Darken2);

                                    c.Item().PaddingTop(8).Grid(grid =>
                                    {
                                        grid.Columns(2);

                                        AddDetailBox(grid, "Job Number", model.JobNumber);
                                        AddDetailBox(grid, "Booked By", model.BookedBy);
                                        AddDetailBox(grid, "Scheduled Time", model.ScheduledTime);
                                        AddDetailBox(grid, "Asset", model.Asset);
                                        AddDetailBox(grid, "Serial Number", model.SerialNumber);
                                        AddDetailBox(grid, "Model Number", model.ModelNumber);
                                        AddDetailBox(grid, "Job Started", model.JobStartedTime);
                                        AddDetailBox(grid, "Job Completed", model.JobCompletedTime);
                                    });
                                });

                            main.Item().PaddingVertical(10);

                            AddTextSection(main, "Scope Of Work", model.ScopeOfWork);
                            AddTextSection(main, "Notes", model.Notes);
                            AddTextSection(main, "Technician Notes", model.TechnicianNotes);

                            // ================= SIGNATURE =================
                            main.Item().Container()
                                .Background(Colors.Grey.Lighten5)
                                .Padding(12)
                                .Column(c =>
                                {
                                    c.Item().Text("Acknowledgement")
                                        .Bold()
                                        .FontSize(12)
                                        .FontColor(Colors.Blue.Darken2);

                                    c.Item().PaddingTop(15).Row(row =>
                                    {
                                        row.RelativeItem().Column(col =>
                                        {
                                            col.Item().Height(40).BorderBottom(1);
                                            col.Item().Text("Customer Signature").FontSize(9);
                                        });

                                        row.RelativeItem().Column(col =>
                                        {
                                            col.Item().Height(40).BorderBottom(1);
                                            col.Item().Text("Technician Signature").FontSize(9);
                                        });
                                    });
                                });

                            AddImageSection(main, "Before Images", model.BeforeImages);
                            AddImageSection(main, "After Images", model.AfterImages);

                            // ================= FOOTER =================
                            main.Item().AlignCenter().PaddingTop(15).Text(text =>
                            {
                                text.Span("Generated on ")
                                    .FontSize(8)
                                    .FontColor(Colors.Grey.Darken1);

                                text.Span(DateTime.Now.ToString("dd-MMM-yyyy HH:mm"))
                                    .Bold()
                                    .FontSize(8);

                                text.Span(" | Page ");
                                text.CurrentPageNumber();
                                text.Span(" of ");
                                text.TotalPages();
                            });
                        });
                    });
                });
            }).GeneratePdf();
        }

        // ================= HELPER METHODS =================

        private void InfoLine(ColumnDescriptor column, string label, string value)
        {
            column.Item().Row(row =>
            {
                row.RelativeItem(1).Text(label).SemiBold();
                row.RelativeItem(2).Text(value ?? "-");
            });
        }

        private void AddDetailBox(GridDescriptor grid, string title, string value)
        {
            grid.Item().Padding(5).Border(1)
                .BorderColor(Colors.Grey.Lighten3)
                .Padding(8)
                .Column(col =>
                {
                    col.Item().Text(title)
                        .FontSize(9)
                        .SemiBold()
                        .FontColor(Colors.Grey.Darken1);

                    col.Item().PaddingTop(3)
                        .Text(value ?? "-")
                        .FontSize(10)
                        .Bold();
                });
        }

        private void AddTextSection(ColumnDescriptor column, string title, string value)
        {
            column.Item().Container()
                .Background(Colors.Grey.Lighten5)
                .Padding(12)
                .Column(c =>
                {
                    c.Item().Text(title)
                        .Bold()
                        .FontSize(12)
                        .FontColor(Colors.Blue.Darken2);

                    c.Item().PaddingTop(5)
                        .Text(value ?? "-");
                });
        }

        private void AddImageSection(ColumnDescriptor column, string title, List<byte[]> images)
        {
            if (images == null || images.Count == 0)
                return;

            column.Item().PageBreak();

            column.Item().Text(title)
                .Bold()
                .FontSize(13)
                .FontColor(Colors.Blue.Darken2);

            column.Item().PaddingTop(10).Grid(grid =>
            {
                grid.Columns(3);

                foreach (var image in images)
                {
                    if (image != null && image.Length > 0)
                    {
                        grid.Item()
                            .Padding(5)
                            .Height(130)
                            .Background(Colors.Grey.Lighten5)
                            .AlignCenter()
                            .AlignMiddle()
                            .Image(image, ImageScaling.FitArea);
                    }
                }
            });
        }
    }
}