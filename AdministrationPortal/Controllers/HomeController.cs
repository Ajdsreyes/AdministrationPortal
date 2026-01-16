using System.Diagnostics;
using AdministrationPortal.Models;
using AdministrationPortal.Data; // Ensure this points to where your ApplicationDbContext is
using Microsoft.AspNetCore.Mvc;

namespace AdministrationPortal.Controllers
{
    public class HomeController : Controller
    {
        // 1. This private variable holds the database connection
        private readonly ApplicationDbContext _context;

        // 2. The Constructor: This "injects" the database into the controller
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // 3. This fetches the list of users from your SQL table [Users]
            var users = _context.Users.ToList();
            return View(users);
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
}