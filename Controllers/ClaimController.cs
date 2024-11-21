using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ClaimSystemMVC.Models;

namespace ClaimSystemMVC.Controllers
{
    public static class ClaimStorage
    {
        // In-memory storage for claims
        public static List<ClaimModel> Claims { get; set; } = new List<ClaimModel>();
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
                // Calculate the total amount
                claim.Amount = CalculateClaimAmount(claim.HoursWorked, claim.HourlyRate);

                // Assign default values
                claim.ClaimId = ClaimStorage.Claims.Count + 1;
                claim.LecturerId = User.Identity?.Name; // Use the logged-in user's identity
                claim.Status = "Pending";
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

            // Ensure the logged-in lecturer can only view their claims
            if (claim.LecturerId != User.Identity?.Name)
            {
                return Unauthorized();
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
            var lecturerId = User.Identity?.Name;

            // Filter claims by logged-in lecturer
            var claims = ClaimStorage.Claims
                .Where(c => c.LecturerId == lecturerId)
                .ToList();

            return View(claims);
        }

        // GET: Claim/Pending (View all pending claims)
        [Authorize(Roles = "Coordinator,AcademicManager")]
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
        [Authorize(Roles = "Coordinator,AcademicManager")]
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
        [Authorize(Roles = "Coordinator,AcademicManager")]
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
        [Authorize(Roles = "Coordinator,AcademicManager")]
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
