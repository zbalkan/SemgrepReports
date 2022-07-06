﻿using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SemgrepReports.Components;
using SemgrepReports.Models;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace SemgrepReports
{
    internal static class ReportGenerator
    {
        public static Report Import(string input)
        {
            var jsonString = File.ReadAllText(input);
            var report = JsonSerializer.Deserialize<Report>(jsonString);
            return report;
        }

        public static void Export(Report report, string output)
        {
            Document.Create(container =>
            {
                _ = container.Page(page =>
                {
                    SetupPage(page);
                    GenerateHeader(report, page);
                    GenerateContent(report, page);
                    GenerateFooter(report, page);
                });
            })
                .GeneratePdf(output);
        }

        private static void SetupPage(PageDescriptor page)
        {
            page.Size(PageSizes.A4);
            page.Margin(2, Unit.Centimetre);
            page.PageColor(Colors.White);

            var textStyle = new TextStyle()
                .FontSize(11)
                .FontFamily("Times New Roman");

            page.DefaultTextStyle(textStyle);
        }

        private static void GenerateHeader(Report report, PageDescriptor page)
        {
            page
                .Header()
                .Text($"Static Application Security Testing (SAST) Report (v{report.Version})")
                .H1();
        }

        private static void GenerateContent(Report report, PageDescriptor page)
        {
            var vulns = report
                .Vulnerabilities
                .OrderBy(x => x.Priority)
                .ThenBy(x => x.Location.File)
                .ToList();

            page
                .Content()
                .Column(column => {
                    column.Item().Component(new ReportMetadata(report));
                    column.Item().Component(new ExecutiveSummary(vulns));

                    column.Item().PageBreak();
                    column.Item().Text("Findings").H2();

                    foreach (var vuln in vulns)
                    {
                        column.Item().Component(new Finding(vuln));
                    }
                });
        }
   
        private static void GenerateFooter(Report report, PageDescriptor page)
        {
            page
                .Footer()
                .AlignCenter()
                .Text(x =>
                {
                    x.Span("Page ");
                    x.CurrentPageNumber();
                    x.Span(" of ");
                    x.TotalPages();
                    x.EmptyLine();
                    x.Span($"(v{report.Version})");
                });
        }
    }
}
