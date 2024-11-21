using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ClaimSystemMVC.Data;
using ClaimSystemMVC.Models;

namespace ClaimSystemMVC.Controllers
{
    public class ClaimController : Controller
    {
        // GET: Claim/Create (Form for creating a new claim)
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Claim/Create (Handles claim submission)
        [HttpPost]
        public IActionResult Create(ClaimModel claim)
        {
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
                // Auto-increment ClaimId
                claim.ClaimId = ClaimStorage.Claims.Count + 1;  // Increment based on existing claims

                // Perform the calculation
                claim.Amount = claim.HoursWorked * claim.HourlyRate;

                claim.Status = "Pending";  // Set status to Pending
                claim.SubmissionDate = DateTime.Now;

                ClaimStorage.Claims.Add(claim);

                // Redirect to the lecturer's status page
                return RedirectToAction("Status");
            }
            return View(claim);
        }

        //GET: View the claim status
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

        // GET: Claim/Status (Lecturer's claim status page)
        public IActionResult Status()
        {
            var lecturerId = User.Identity?.Name;
            var claims = ClaimStorage.Claims
                .Where(c => c.LecturerId == lecturerId) // Get claims for logged-in lecturer
                .ToList();

            return View(claims);
        }
    }
}