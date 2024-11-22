using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ClaimSystemMVC.Models;

namespace ClaimSystemMVC.Controllers
{
    public static class ClaimStorage
    {
        // In-memory storage for claims and lecturers
        public static List<ClaimModel> Claims { get; set; } = new List<ClaimModel>();
        public static List<Lecturer> Lecturers { get; set; } = new List<Lecturer>();
    }

    public class ClaimController : Controller
    {
        // GET: Claim/Create (Form for creating a new claim)
        [HttpGet]
        public IActionResult Create()
        {
            return View(new ClaimModel());
        }

        // POST: Claim/Create (Handles claim submission)
        [HttpPost]
        public IActionResult Create(ClaimModel claim)
        {
            // Validate input
            if (claim.HoursWorked <= 0)
            {
                ModelState.AddModelError("HoursWorked", "Hours worked must be a positive number.");
            }

            if (claim.HourlyRate <= 0)
            {
                ModelState.AddModelError("HourlyRate", "Hourly rate must be a positive number.");
            }

            if (ModelState.IsValid)
            {
                // Check if the claim has a document attached
                if (claim.SupportingDocumentPath == null)
                {
                    // Automatically reject the claim if no document is attached
                    claim.Status = "Rejected";
                    claim.RejectionReason = "No document attached.";
                }
                else
                {
                    // Calculate the total amount
                    claim.Amount = CalculateClaimAmount(claim.HoursWorked, claim.HourlyRate);
                    claim.Status = "Pending";
                }

                // Assign default values (no lecturer identity)
                claim.ClaimId = ClaimStorage.Claims.Count + 1;
                claim.LecturerId = "UnknownLecturer"; // Lecturer identity removed, set a default value
                claim.SubmissionDate = DateTime.Now;

                // Store the claim in memory
                ClaimStorage.Claims.Add(claim);

                TempData["SuccessMessage"] = "Claim submitted successfully!";
                return RedirectToAction("Status");
            }

            return View(claim);
        }

        // GET: Claim/ViewClaim/{id} (View individual claim)
        [HttpGet]
        public IActionResult ViewClaim(int id)
        {
            var claim = ClaimStorage.Claims.FirstOrDefault(c => c.ClaimId == id);

            if (claim == null)
            {
                return NotFound();
            }

            return View(claim);
        }

        private decimal CalculateClaimAmount(int hoursWorked, decimal hourlyRate)
        {
            return hoursWorked * hourlyRate;
        }

        // GET: Claim/Status (Lecturer's claim status page)
        public IActionResult Status()
        {
            // No login, using a default or fixed lecturer ID (remove logic for logged-in lecturer)
            var claims = ClaimStorage.Claims.ToList();
            return View(claims);
        }

        // GET: Claim/Pending (View all pending claims)
        [HttpGet]
        public IActionResult Pending()
        {
            // Get all claims with "Pending" status
            var pendingClaims = ClaimStorage.Claims
                .Where(c => c.Status == "Pending")
                .ToList();

            return View(pendingClaims);
        }

        // GET: Claim/Review/{id} (View individual claim for review)
        [HttpGet]
        public IActionResult Review(int id)
        {
            var claim = ClaimStorage.Claims.FirstOrDefault(c => c.ClaimId == id);

            if (claim == null)
            {
                return NotFound();
            }

            return View(claim);
        }

        // POST: Claim/Approve/{id} (Approve a claim)
        [HttpPost]
        public IActionResult Approve(int id)
        {
            var claim = ClaimStorage.Claims.FirstOrDefault(c => c.ClaimId == id);

            if (claim == null)
            {
                return NotFound();
            }

            // Update claim status to "Approved"
            claim.Status = "Approved";
            claim.ApprovedDate = DateTime.Now;

            TempData["SuccessMessage"] = $"Claim {id} approved successfully.";
            return RedirectToAction("Pending");
        }

        // POST: Claim/Reject/{id} (Reject a claim)
        [HttpPost]
        public IActionResult Reject(int id, string rejectionReason)
        {
            var claim = ClaimStorage.Claims.FirstOrDefault(c => c.ClaimId == id);

            if (claim == null)
            {
                return NotFound();
            }

            // Update claim status to "Rejected" and include the reason
            claim.Status = "Rejected";
            claim.RejectionReason = rejectionReason;

            TempData["ErrorMessage"] = $"Claim {id} rejected.";
            return RedirectToAction("Pending");
        }
    }
}
