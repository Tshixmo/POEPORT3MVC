using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimSystemMVC.Models
{
    public class ClaimModel
    {
        public int ClaimId { get; set; }  
        public string? LecturerId { get; set; }
        public decimal Amount { get; set; }
        public int HoursWorked { get; set; }
        public decimal HourlyRate { get; set; }
        public string? Status { get; set; }
        public string? SupportingDocumentPath { get; set; }
        public string? AdditionalNotes { get; set; }
        public DateTime SubmissionDate { get; set; }
    }
}