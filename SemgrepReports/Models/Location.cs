﻿using System.Text.Json.Serialization;

namespace SemgrepReports.Models
{
    public class Location
    {
        [JsonPropertyName("file")]
        public string File { get; set; }

        [JsonPropertyName("commit")]
        public Commit Commit { get; set; }

        [JsonPropertyName("start_line")]
        public int StartLine { get; set; }
    }
}
