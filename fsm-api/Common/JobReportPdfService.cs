using fsm_api.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
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
                    page.Margin(20);

                    page.Content().Column(column =>
                    {
                        column.Spacing(8);

                        // HEADER
                        column.Item().Text($"Job Report - {model.JobNumber} {model.BookedTime}")
                            .FontSize(16).Bold();

                        column.Item().LineHorizontal(1);

                        // CONTACT INFO
                        column.Item().Text("Contact Information").Bold();
                        column.Item().Text($"Company: {model.CompanyName}");
                        column.Item().Text($"Technician: {model.Technician}");
                        column.Item().Text($"Status: {model.Status}");
                        column.Item().Text($"Location: {model.Location}");

                        column.Item().LineHorizontal(1);

                        // JOB DETAILS TABLE
                        column.Item().Text("Job Details").Bold();

                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(2);
                            });

                            AddRow(table, "Job Number", model.JobNumber);
                            AddRow(table, "Business Unit", model.BusinessUnit);
                            AddRow(table, "Service Type", model.ServiceType);
                            AddRow(table, "Booked Time", model.BookedTime);
                            AddRow(table, "Scheduled Time", model.ScheduledTime);
                            AddRow(table, "Booked By", model.BookedBy);
                            AddRow(table, "Asset", model.Asset);
                            AddRow(table, "Serial Number", model.SerialNumber);
                            AddRow(table, "Model Number", model.ModelNumber);
                            AddRow(table, "Job Started Time", model.JobStartedTime);
                            AddRow(table, "Job Completed Time", model.JobCompletedTime);
                            AddRow(table, "Total Job Time", model.TotalJobTime);
                            AddRow(table, "No. Of Crew", model.NoOfCrew.ToString());
                            AddRow(table, "Job Title", model.JobTitle);
                        });

                        column.Item().LineHorizontal(1);

                        // SCOPE / NOTES
                        column.Item().Text("Scope Of Work").Bold();
                        column.Item().Text(model.ScopeOfWork ?? "-");

                        column.Item().Text("Notes").Bold();
                        column.Item().Text(model.Notes ?? "-");

                        column.Item().Text("Technician Notes").Bold();
                        column.Item().Text(model.TechnicianNotes ?? "-");

                        // BEFORE IMAGES PAGE
                        if (model.BeforeImageUrls != null && model.BeforeImageUrls.Count > 0)
                        {
                            column.Item().PageBreak();
                            column.Item().Text("Before Images").Bold();

                            column.Item().Grid(grid =>
                            {
                                grid.Columns(2);

                                foreach (var url in model.BeforeImageUrls)
                                {
                                    var imageBytes = DownloadImage(url);
                                    if (imageBytes != null)
                                        grid.Item().Padding(5).Image(imageBytes, ImageScaling.FitArea);
                                }
                            });
                        }

                        // AFTER IMAGES PAGE
                        if (model.AfterImageUrls != null && model.AfterImageUrls.Count > 0)
                        {
                            column.Item().PageBreak();
                            column.Item().Text("After Images").Bold();

                            column.Item().Grid(grid =>
                            {
                                grid.Columns(2);

                                foreach (var url in model.AfterImageUrls)
                                {
                                    var imageBytes = DownloadImage(url);
                                    if (imageBytes != null)
                                        grid.Item().Padding(5).Image(imageBytes, ImageScaling.FitArea);
                                }
                            });
                        }
                    });
                });
            }).GeneratePdf();
        }

        private void AddRow(TableDescriptor table, string label, string value)
        {
            table.Cell().Border(1).Padding(5).Text(label).Bold();
            table.Cell().Border(1).Padding(5).Text(value ?? "-NA-");
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