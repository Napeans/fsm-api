using fsm_api.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Net;

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
                    page.Margin(30);

                    page.Header().Column(header =>
                    {
                        header.Item().Text($"Job Report - {model.JobNumber} {model.BookedTime}")
                            .FontSize(18)
                            .Bold();

                        header.Item().LineHorizontal(1);
                    });

                    page.Content().Column(column =>
                    {
                        column.Spacing(15);

                        // ================= CONTACT INFORMATION BOX =================
                        column.Item().Container()
                            .Border(1)
                            .Padding(10)
                            .Column(c =>
                            {
                                c.Item().Text("Contact Information").Bold().FontSize(14);

                                c.Item().Text($"Company: {model.CompanyName}");
                                c.Item().Text($"Technician: {model.Technician}");
                                c.Item().Text($"Status: {model.Status}");
                                c.Item().Text($"Location: {model.Location}");
                            });

                        // ================= JOB DETAILS TABLE BOX =================
                        column.Item().Container()
                            .Border(1)
                            .Padding(10)
                            .Column(c =>
                            {
                                c.Item().Text("Job Details").Bold().FontSize(14);

                                c.Item().Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.ConstantColumn(180);
                                        columns.RelativeColumn();
                                    });

                                    AddStyledRow(table, "Job Number", model.JobNumber);
                                    AddStyledRow(table, "Business Unit", model.BusinessUnit);
                                    AddStyledRow(table, "Service Type", model.ServiceType);
                                    AddStyledRow(table, "Booked Time", model.BookedTime);
                                    AddStyledRow(table, "Scheduled Time", model.ScheduledTime);
                                    AddStyledRow(table, "Booked By", model.BookedBy);
                                    AddStyledRow(table, "Asset", model.Asset);
                                    AddStyledRow(table, "Serial Number", model.SerialNumber);
                                    AddStyledRow(table, "Model Number", model.ModelNumber);
                                    AddStyledRow(table, "Job Started Time", model.JobStartedTime);
                                    AddStyledRow(table, "Job Completed Time", model.JobCompletedTime);
                                    AddStyledRow(table, "Total Job Time", model.TotalJobTime);
                                    AddStyledRow(table, "No. Of Crew", model.NoOfCrew.ToString());
                                    AddStyledRow(table, "Job Title", model.JobTitle);
                                });
                            });

                        // ================= TEXT SECTIONS =================
                        AddTextSection(column, "Scope Of Work", model.ScopeOfWork);
                        AddTextSection(column, "Notes", model.Notes);
                        AddTextSection(column, "Technician Notes", model.TechnicianNotes);

                        // ================= BEFORE IMAGES =================
                        if (model.BeforeImages?.Count > 0)
                        {
                            column.Item().PageBreak();

                            column.Item().Text("Before Images")
                                .FontSize(14)
                                .Bold();

                            column.Item().LineHorizontal(1);

                            column.Item().Grid(grid =>
                            {
                                grid.Columns(2);

                                foreach (var imageBytes in model.BeforeImages)
                                {
                                    if (imageBytes != null && imageBytes.Length > 0)
                                    {
                                        grid.Item()
                                            .Padding(5)
                                            .Border(1)
                                            .Height(180)
                                            .Image(imageBytes, ImageScaling.FitArea);
                                    }
                                }
                            });
                        }

                        // ================= AFTER IMAGES =================
                        if (model.AfterImages?.Count > 0)
                        {
                            column.Item().PageBreak();

                            column.Item().Text("After Images")
                                .FontSize(14)
                                .Bold();

                            column.Item().LineHorizontal(1);

                            column.Item().Grid(grid =>
                            {
                                grid.Columns(2);

                                foreach (var imageBytes in model.AfterImages)
                                {
                                    if (imageBytes != null && imageBytes.Length > 0)
                                    {
                                        grid.Item()
                                            .Padding(5)
                                            .Border(1)
                                            .Height(180)
                                            .Image(imageBytes, ImageScaling.FitArea);
                                    }
                                }
                            });
                        }
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text(text =>
                        {
                            text.Span("Generated on ");
                            text.Span(DateTime.Now.ToString("dd-MMM-yyyy HH:mm"));
                        });
                });
            }).GeneratePdf();
        }

        private void AddStyledRow(TableDescriptor table, string label, string value)
        {
            table.Cell().Border(1).Padding(5).Text(label).Bold();
            table.Cell().Border(1).Padding(5).Text(value ?? "-NA-");
        }

        private void AddTextSection(ColumnDescriptor column, string title, string value)
        {
            column.Item().Container()
                .Border(1)
                .Padding(10)
                .Column(c =>
                {
                    c.Item().Text(title).Bold().FontSize(14);
                    c.Item().Text(value ?? "-");
                });
        }

        private byte[] DownloadImage(string url)
        {
            try
            {
                using (var wc = new WebClient())
                {
                    return wc.DownloadData(url);
                }
            }
            catch
            {
                return null;
            }
        }
    }
}