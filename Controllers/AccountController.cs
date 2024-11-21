using ClaimSystemMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

public class AccountController : Controller
{
    // GET: Account/Register
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    // POST: Account/Register
    [HttpPost]
    public IActionResult Register(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            ViewBag.ErrorMessage = "Username and password are required.";
            return View();
        }

        if (UserStorage.Users.Any(u => u.Username == username))
        {
            ViewBag.ErrorMessage = "Username already exists.";
            return View();
        }

        UserStorage.Users.Add(new UserModel
        {
            Username = username,
            Password = password // Note: In production, always hash passwords!
        });

        ViewBag.SuccessMessage = "Registration successful. Please log in.";
        return RedirectToAction("Login");
    }

    // GET: Account/Login
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    // POST: Account/Login
    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        var user = UserStorage.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
        if (user == null)
        {
            ViewBag.ErrorMessage = "Invalid username or password.";
            return View();
        }

        // Simulate login by using session or temp data
        TempData["LoggedInUser"] = username;

        return RedirectToAction("Index", "Home");
    }

    // POST: Account/Logout
    [HttpPost]
    public IActionResult Logout()
    {
        TempData["LoggedInUser"] = null;
        return RedirectToAction("Login");
    }
}
