using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ImaginationStation.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ImaginationStation.Controllers;

public class LoginController : Controller
{

    private MyContext _context;

    public LoginController(MyContext context)
    {
        _context = context;
    }

    // =======HOME PAGE READ==========
    [HttpGet("")]
    public IActionResult Index()
    {
        // check if logged in and if so, redirect to wedding dashboard page
        if(HttpContext.Session.GetInt32("UserId") != null)
        {
            return RedirectToAction("All", "Post");
        }

        return View("Index");
    }

    // =========REGISTER ACTION========
    [HttpPost("users/create")]
    public IActionResult CreateUser(User newUser)
    {
        if (!ModelState.IsValid)
        {
            return View("Index");
        }

        PasswordHasher<User> hashpass = new PasswordHasher<User>();
        newUser.Password = hashpass.HashPassword(newUser, newUser.Password);

        _context.Users.Add(newUser);
        _context.SaveChanges();

        HttpContext.Session.SetInt32("UserId", newUser.UserId);
        HttpContext.Session.SetString("Name", newUser.Name);

        return RedirectToAction("All", "Post");
    }

    // ========LOGIN ACTION=======
    [HttpPost("login")]
    public IActionResult Login(LoginUser userSubmission)
    {
        if (!ModelState.IsValid)
        {
            return View("Index");
        }
        // If initial ModelState is valid, query for a user with the provided email        
        User? userInDb = _context.Users.FirstOrDefault(u => u.Email == userSubmission.LoginEmail);
        // If no user exists with the provided email        
        if (userInDb == null)
        {
            // Add an error to ModelState and return to View!            
            ModelState.AddModelError("LoginEmail", "Invalid Email/Password");
            return View("Index");
        }
        // Otherwise, we have a user, now we need to check their password                 
        // Initialize hasher object        
        PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();
        // Verify provided password against hash stored in db        
        var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.LoginPassword);                                    // Result can be compared to 0 for failure        
        if (result == 0)
        {
            // Handle failure (this should be similar to how "existing email" is handled)  
            ModelState.AddModelError("LoginEmail", "Invalid Email/Password");
            return View("Index");
        }
        // Handle success (this should route to an internal page)  
        HttpContext.Session.SetInt32("UserId", userInDb.UserId);
        HttpContext.Session.SetString("Name", userInDb.Name);

        return RedirectToAction("All", "Post");
    }

    // ========LOGOUT USER======
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
public class SessionCheckAttribute : ActionFilterAttribute
{
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        int? userId = context.HttpContext.Session.GetInt32("UserId");

        if (userId == null)
        {
            context.Result = new RedirectToActionResult("Index", "Login", null);
        }
    }
}