﻿using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using SemgrepReports.Models.SecretLeakCheck;

namespace SemgrepReports.Components
{
    internal sealed class TitlePage : IComponent
    {
        private readonly SecretLeakCheckReport _report;

        public TitlePage(SecretLeakCheckReport report)
        {
            _report = report;
        }

        public void Compose(IContainer container) => container
                .Decoration(decoration => decoration
                        .Content()
                        .AlignCenter()
                        .PaddingTop(10, Unit.Centimetre)
                        .Text($"Static Application Security Testing (SAST) Report\n(v{_report.Version})")
                        .Title());
    }
}
