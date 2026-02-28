using fsm_api.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Net;

namespace fsm_api.Common
{
    public class JobReportPdfService
    {
        public byte[] Generate(JobReportModel model)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var primaryColor = Colors.Blue.Medium;
            var lightGray = Colors.Grey.Lighten3;

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(25);

                    // ================= HEADER BAR =================
                    page.Header().Container()
                        .Background(primaryColor)
                        .Padding(15)
                        .Column(header =>
                        {
                            header.Item().Text($"JOB REPORT")
                                .FontSize(20)
                                .Bold()
                                .FontColor(Colors.White);

                            header.Item().Text($"{model.JobNumber} | {model.BookedTime}")
                                .FontSize(12)
                                .FontColor(Colors.White);
                        });

                    page.Content().PaddingVertical(15).Column(column =>
                    {
                        column.Spacing(20);

                        // ================= CONTACT INFO =================
                        column.Item().Container()
                            .Border(1)
                            .BorderColor(lightGray)
                            .Padding(15)
                            .Column(c =>
                            {
                                SectionTitle(c, "Contact Information");

                                InfoRow(c, "Company", model.CompanyName);
                                InfoRow(c, "Technician", model.Technician);
                                InfoRow(c, "Status", model.Status);
                                InfoRow(c, "Location", model.Location);
                            });

                        // ================= JOB DETAILS =================
                        column.Item().Container()
                            .Border(1)
                            .BorderColor(lightGray)
                            .Padding(15)
                            .Column(c =>
                            {
                                SectionTitle(c, "Job Details");

                                c.Item().Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn(1);
                                        columns.RelativeColumn(2);
                                    });

                                    AddColoredRow(table, "Job Number", model.JobNumber);
                                    AddColoredRow(table, "Business Unit", model.BusinessUnit);
                                    AddColoredRow(table, "Service Type", model.ServiceType);
                                    AddColoredRow(table, "Booked Time", model.BookedTime);
                                    AddColoredRow(table, "Scheduled Time", model.ScheduledTime);
                                    AddColoredRow(table, "Booked By", model.BookedBy);
                                    AddColoredRow(table, "Asset", model.Asset);
                                    AddColoredRow(table, "Serial Number", model.SerialNumber);
                                    AddColoredRow(table, "Model Number", model.ModelNumber);
                                    AddColoredRow(table, "Job Started Time", model.JobStartedTime);
                                    AddColoredRow(table, "Job Completed Time", model.JobCompletedTime);
                                    AddColoredRow(table, "Total Job Time", model.TotalJobTime);
                                    AddColoredRow(table, "No. Of Crew", model.NoOfCrew.ToString());
                                    AddColoredRow(table, "Job Title", model.JobTitle);
                                });
                            });

                        // ================= TEXT SECTIONS =================
                        AddStyledTextSection(column, "Scope Of Work", model.ScopeOfWork);
                        AddStyledTextSection(column, "Notes", model.Notes);
                        AddStyledTextSection(column, "Technician Notes", model.TechnicianNotes);

                        // ================= BEFORE IMAGES =================
                        if (model.BeforeImages?.Count > 0)
                        {
                            column.Item().PageBreak();
                            SectionTitle(column, "Before Images");

                            AddImageGrid(column, model.BeforeImages);
                        }

                        // ================= AFTER IMAGES =================
                        if (model.AfterImages?.Count > 0)
                        {
                            column.Item().PageBreak();
                            SectionTitle(column, "After Images");

                            AddImageGrid(column, model.AfterImages);
                        }
                    });

                    // ================= FOOTER =================
                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("Generated on ")
                            .FontSize(9)
                            .FontColor(Colors.Grey.Darken1);

                        text.Span(DateTime.Now.ToString("dd-MMM-yyyy HH:mm"))
                            .FontSize(9)
                            .Bold();
                    });
                });
            }).GeneratePdf();
        }
        private void SectionTitle(ColumnDescriptor column, string title)
        {
            column.Item().Container()
                .Background(Colors.Blue.Lighten4)
                .Padding(6)
                .Text(title)
                .Bold()
                .FontSize(14)
                .FontColor(Colors.Blue.Darken2);
        }

        private void InfoRow(ColumnDescriptor column, string label, string value)
        {
            column.Item().Row(row =>
            {
                row.RelativeItem(1).Text(label).Bold();
                row.RelativeItem(2).Text(value ?? "-");
            });
        }

        private void AddColoredRow(TableDescriptor table, string label, string value)
        {
            table.Cell()
                .Background(Colors.Grey.Lighten4)
                .Padding(6)
                .Text(label)
                .Bold();

            table.Cell()
                .Padding(6)
                .Text(value ?? "-");
        }

        private void AddStyledTextSection(ColumnDescriptor column, string title, string value)
        {
            column.Item().Container()
                .Border(1)
                .BorderColor(Colors.Grey.Lighten3)
                .Padding(12)
                .Column(c =>
                {
                    SectionTitle(c, title);
                    c.Item().PaddingTop(5).Text(value ?? "-");
                });
        }

        private void AddImageGrid(ColumnDescriptor column, List<byte[]> images)
        {
            column.Item().Grid(grid =>
            {
                grid.Columns(2);

                foreach (var imageBytes in images)
                {
                    if (imageBytes != null && imageBytes.Length > 0)
                    {
                        grid.Item()
                            .Padding(8)
                            .Border(1)
                            .BorderColor(Colors.Grey.Lighten2)
                            .Background(Colors.Grey.Lighten5)
                            .Height(180)
                            .AlignCenter()
                            .AlignMiddle()
                            .Image(imageBytes, ImageScaling.FitArea);
                    }
                }
            });
        }
    }
}