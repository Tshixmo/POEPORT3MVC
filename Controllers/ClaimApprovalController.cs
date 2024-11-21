using Microsoft.AspNetCore.Mvc;
using ClaimSystemMVC.Data;
using ClaimSystemMVC.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace ClaimSystemMVC.Controllers
{
    [Authorize(Roles = "Coordinator, AcademicManager")] // Only accessible to authorized roles
    public class ClaimApprovalController : Controller
    {
        // GET: ClaimApproval/Index - List of pending claims
        public IActionResult Index()
        {
            var pendingClaims = ClaimStorage.Claims
                .Where(c => c.Status == "Pending")
                .ToList();
            return View(pendingClaims);
        }

        // GET: ClaimApproval/Details/{id} - View individual claim details
        [HttpGet]
        public IActionResult Details(int id)
        {
            var claim = ClaimStorage.Claims.FirstOrDefault(c => c.ClaimId == id);
            if (claim == null)
            {
                return NotFound();
            }
            return View(claim);
        }

        // POST: ClaimApproval/Approve/{id} - Approve a claim
        [HttpPost]
        public IActionResult Approve(int id)
        {
            var claim = ClaimStorage.Claims.FirstOrDefault(c => c.ClaimId == id);
            if (claim == null)
            {
                return NotFound();
            }

            // Approve claim
            claim.Status = "Approved";

            TempData["SuccessMessage"] = "Claim approved successfully.";
            return RedirectToAction("Index");
        }

        // POST: ClaimApproval/Reject/{id} - Reject a claim
        [HttpPost]
        public IActionResult Reject(int id, string rejectionReason)
        {
            var claim = ClaimStorage.Claims.FirstOrDefault(c => c.ClaimId == id);
            if (claim == null)
            {
                return NotFound();
            }

            // Reject claim
            claim.Status = "Rejected";
            claim.RejectionReason = rejectionReason;

            TempData["ErrorMessage"] = $"Claim {id} rejected for the following reason: {rejectionReason}";
            return RedirectToAction("Index");
        }

        // GET: ClaimApproval/History - View all processed claims
        [HttpGet]
        public IActionResult History()
        {
            var processedClaims = ClaimStorage.Claims
                .Where(c => c.Status == "Approved" || c.Status == "Rejected")
                .ToList();
            return View(processedClaims);
        }
    }
}
