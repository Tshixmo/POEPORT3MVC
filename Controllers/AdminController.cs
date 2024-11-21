using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ClaimSystemMVC.Data;

namespace ClaimSystemMVC.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin/Dashboard (Shows all pending claims for approval or rejection)
        public IActionResult Dashboard()
        {
            var pendingClaims = ClaimStorage.Claims
                .Where(c => c.Status == "Pending") // Get pending claims
                .ToList();

            return View(pendingClaims);
        }

        // POST: Admin/Approve (Approve a specific claim)
        [HttpPost]
        public IActionResult Approve(int id)
        {
            var claim = ClaimStorage.Claims.FirstOrDefault(c => c.ClaimId == id);
            if (claim != null)
            {
                claim.Status = "Approved"; // Update the status to Approved
            }
            return RedirectToAction("Dashboard");
        }

        // POST: Admin/Reject (Reject a specific claim)
        [HttpPost]
        public IActionResult Reject(int id)
        {
            var claim = ClaimStorage.Claims.FirstOrDefault(c => c.ClaimId == id);
            if (claim != null)
            {
                claim.Status = "Rejected"; // Update the status to Rejected
            }
            return RedirectToAction("Dashboard");
        }
    }
}
