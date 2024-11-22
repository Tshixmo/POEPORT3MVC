using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ClaimSystemMVC.Models;
using ClaimSystemMVC.Utils;

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
        public IActionResult LogIn()
        {
            return View();
        }

        // POST: Home/LogIn
        [HttpPost]
        public IActionResult LogIn(LoginModel model)
        {
            string username = model.Username;
            string password = model.Password;

            int dbResult = DB.LogUserIn(model);
            if (dbResult != 0)
            {
                // Get user details after login
                var user = DB.GetUserByUsername(username); // You should create this method in your DB class to fetch user info based on username

                // Check the role and redirect accordingly
                if (user.Role == "admin")
                {
                    return RedirectToAction("Index", "AdminDashboard"); // Admin dashboard action
                }
                else if (user.Role == "user")
                {
                    return RedirectToAction("Index", "LecturerDashboard"); // Lecturer dashboard action
                }
                else if (user.Role == "hr")
                {
                    return RedirectToAction("Index", "HrDashboard"); // HR dashboard action
                }
                else
                {
                    TempData["ErrorMessage"] = "Unknown role. Access denied.";
                    return RedirectToAction("LogIn"); // Stay on login page if no role is matched
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid login credentials.";
                return RedirectToAction("LogIn"); // Stay on login page if login fails
            }
        }


        // GET: Home/Welcome
        public IActionResult Welcome()
        {
            // Check if the username exists in TempData (or Session)
            if (TempData["Username"] != null)
            {
                string username = TempData["Username"].ToString();
                return View("Welcome", model: username);
            }

            // If no user is logged in, redirect to the login page
            return RedirectToAction("LogIn");
        }

        // Error handling action
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
