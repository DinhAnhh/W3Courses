using System;
using System.Collections.Generic;

#nullable disable

namespace WebLibrary.Models
{
    public partial class Report
    {
        public int ReportId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime? SubmittedTime { get; set; }
    }
}
