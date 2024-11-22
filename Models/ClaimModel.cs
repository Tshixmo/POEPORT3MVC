using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ClaimSystemMVC.Models
{
    public class ClaimModel
    {
        public int ClaimId { get; set; }  
        public string? LecturerId { get; set; }
        public decimal Amount { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Hours worked must be greater than zero.")]
        public int HoursWorked { get; set; }        
        
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Hourly rate must be greater than zero.")]
        public decimal HourlyRate { get; set; }        
        public string? Status { get; set; }
        public SupportingDocument? SupportingDocumentPath { get; set; }
        public string? AdditionalNotes { get; set; }

        public string? RejectionReason { get; set; }
        public DateTime SubmissionDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
    }
}


public class SupportingDocument
{
    public string? FilePath { get; set; }
    public byte[]? Content { get; set; } // You can also store the document content as byte array
}

