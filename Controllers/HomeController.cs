using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ClaimSystemMVC.Models;


namespace ClaimSystemMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // GET: Home/Index
        public IActionResult Index()
        {
            return View();
        }

        // GET: Home/LogIn
        /*public IActionResult LogIn()
        {
            return View();
        }

        // POST: Home/LogIn
        [HttpPost]
        public IActionResult LogIn(LoginModel model, string returnUrl = null)
        {
            // Call the DB method to verify the login
            int role = DB.LogUserIn(model);

            if (role != null)
            {
                // Set success message and user role
                TempData["SuccessMessage"] = "Login successful!";
                TempData["UserRole"] = role;

                // Redirect user based on role or return to requested page
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);  // Redirect to the original page the user wanted
                }

                if (role == 1)
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
                else if (role == 2)
                {
                    return RedirectToAction("Create", "Claim");
                }
                else if (role == 3)
                {
                    return RedirectToAction("GenerateReport", "HR");
                }
            }
            else
            {
                // If login fails, show an error message
                TempData["ErrorMessage"] = "Invalid username or password.";
            }

            // Return to login page if authentication fails
            return View();
        }*/

        // GET: Home/Welcome
        [HttpGet]
        public IActionResult Dashboard()
        {
            if (TempData["UserRole"] == null)
            {
                TempData["ErrorMessage"] = "You must log in first.";
                return RedirectToAction("LogIn");
            }

            // Proceed with showing the dashboard
            return View();
        }

        // Error handling action
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
