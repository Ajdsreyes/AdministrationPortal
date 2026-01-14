using Microsoft.AspNetCore.Mvc;
using AdministrationPortal.Models.ViewModels;
using AdministrationPortal.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BarangayAdminPortal.Controllers
{
    public class AdminController : Controller
    {

        private static List<ServiceVM> Services = new()
{
    new ServiceVM { Id = 1, ServiceName = "Barangay Clearance", Icon="bi-file-earmark-text", IsEnabled=true },
    new ServiceVM { Id = 2, ServiceName = "Barangay Certificate", Icon="bi-file-earmark", IsEnabled=true },
    new ServiceVM { Id = 3, ServiceName = "Cedula", Icon="bi-receipt", IsEnabled=true },
    new ServiceVM { Id = 4, ServiceName = "Barangay ID", Icon="bi-person-badge", IsEnabled=false }
};


        // ======================================================
        // FAKE USERS
        // ======================================================
        private static List<UserVM> FakeUsers = new()
        {
            new UserVM
            {
                Id = 1,
                LastName = "Dela Cruz",
                FirstName = "Juan",
                ContactNumber = "09123456789",
                Barangay = "Malanday",
                City = "Valenzuela",
                Province = "Metro Manila",
                IsActive = true,
                PhotoUrl = "/images/avatar1.png"
            },
            new UserVM
            {
                Id = 2,
                LastName = "Santos",
                FirstName = "Maria",
                ContactNumber = "09987654321",
                Barangay = "Malanday",
                City = "Valenzuela",
                Province = "Metro Manila",
                IsActive = false,
                PhotoUrl = "/images/avatar2.png"
            }
        };

        // ======================================================
        // DASHBOARD
        // ======================================================
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
            {
                return RedirectToAction("Login");
            }

            return View(new AdminDashboardVM
            {
                TotalUsers = 12000,
                TotalRequests = 50,
                ApprovedRequests = 73,
                RejectedRequests = 2,
                ReadyToClaim = 105
            });
        }

        // ======================================================
        // USERS
        // ======================================================
        public IActionResult ManageUsers()
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
                return RedirectToAction("Login");

            return View(FakeUsers);
        }

        public IActionResult ViewUser(int id)
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
                return RedirectToAction("Login");

            return View(FakeUsers.FirstOrDefault(u => u.Id == id));
        }

        public IActionResult EditUser(int id)
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
                return RedirectToAction("Login");

            return View(FakeUsers.FirstOrDefault(u => u.Id == id));
        }

        // ======================================================
        // ACTIVATE / DEACTIVATE USERS
        // ======================================================
        public IActionResult Deactivate(int id)
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
                return RedirectToAction("Login");

            var user = FakeUsers.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                user.IsActive = false;
            }

            return RedirectToAction("ManageUsers");
        }

        public IActionResult Reactivate(int id)
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
                return RedirectToAction("Login");

            var user = FakeUsers.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                user.IsActive = true;
            }

            return RedirectToAction("ManageUsers");
        }


        [HttpPost]
        public IActionResult EditUser(UserVM model)
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
                return RedirectToAction("Login");

            var user = FakeUsers.FirstOrDefault(u => u.Id == model.Id);
            if (user == null) return RedirectToAction("ManageUsers");

            user.FirstName = model.FirstName;
            user.MiddleName = model.MiddleName;
            user.LastName = model.LastName;
            user.Suffix = model.Suffix;
            user.DateOfBirth = model.DateOfBirth;
            user.Sex = model.Sex;
            user.CivilStatus = model.CivilStatus;
            user.Religion = model.Religion;
            user.Street = model.Street;
            user.Barangay = model.Barangay;
            user.City = model.City;
            user.Province = model.Province;
            user.StayYears = model.StayYears;
            user.StayMonths = model.StayMonths;
            user.ContactNumber = model.ContactNumber;
            user.Email = model.Email;

            return RedirectToAction("ViewUser", new { id = model.Id });
        }

        // ======================================================
        // SERVICES
        // ======================================================
        public IActionResult BarangayServices()
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
                return RedirectToAction("Login");

            var services = Services;;
            var requests = GetRequests();

            var model = services.Select(s => new ServiceVM
            {
                Id = s.Id,
                ServiceName = s.ServiceName,
                Icon = s.Icon,
                IsEnabled = s.IsEnabled,
                Pending = requests.Count(r => r.ServiceType == s.ServiceName && r.Status == "Pending"),
                Approved = requests.Count(r => r.ServiceType == s.ServiceName && r.Status == "Approved"),
                Ready = requests.Count(r => r.ServiceType == s.ServiceName && r.Status == "Ready for Claim")
            }).ToList();

            return View(model);
        }

        public IActionResult ServiceRequests(string service)
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
                return RedirectToAction("Login");

            var requests = GetRequests();

            if (!string.IsNullOrEmpty(service))
                requests = requests.Where(r => r.ServiceType == service).ToList();

            ViewBag.ServiceName = service;
            return View(requests);
        }

        public IActionResult ViewServiceRequest(int id)
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
                return RedirectToAction("Login");

            var request = GetRequests().FirstOrDefault(r => r.Id == id);
            if (request == null) return NotFound();

            ViewBag.ServiceName = request.ServiceType;
            return View(request);
        }

        [HttpPost]
        public IActionResult UpdateClaimSchedule(
            int id,
            DateTime? dateToClaim,
            TimeSpan? timeToClaim,
            string status,
            string serviceName)
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
                return RedirectToAction("Login");

            var requests = GetRequests();
            var req = requests.FirstOrDefault(r => r.Id == id);
            if (req == null) return NotFound();

            req.Status = status;
            req.DateToClaim = (status == "Approved" || status == "Ready for Claim") ? dateToClaim : null;
            req.TimeToClaim = (status == "Approved" || status == "Ready for Claim") ? timeToClaim : null;

            SaveRequests(requests);
            return RedirectToAction("ServiceRequests", new { service = serviceName });
        }

        private static Dictionary<int, List<ServiceRequirementVM>> ServiceRequirements =
new()
{
    // =========================================================
    // 1️⃣ BARANGAY CLEARANCE
    // =========================================================
    {
        1,
        new List<ServiceRequirementVM>
        {
            new() {
                Id = 1,
                ServiceId = 1,
                Label = "Purpose",
                FieldType = "select",
                IsRequired = true,
                Options = new()
                {
                    "Proof of Residency",
                    "Indigency",
                    "Local Employment",
                    "Loan",
                    "Scholarships",
                    "Pag-ibig",
                    "Philhealth",
                    "SSS",
                    "NBI",
                    "Police Clearance",
                    "PWD Assistance",
                    "Burial Assistance"
                }
            },

            new() { Id = 2, ServiceId = 1, Label = "Last Name", FieldType = "text", IsRequired = true },
            new() { Id = 3, ServiceId = 1, Label = "First Name", FieldType = "text", IsRequired = true },
            new() { Id = 4, ServiceId = 1, Label = "Middle Name", FieldType = "text", IsRequired = false },
            new() { Id = 5, ServiceId = 1, Label = "Suffix", FieldType = "text", IsRequired = false },

            new() { Id = 6, ServiceId = 1, Label = "Date of Birth", FieldType = "date", IsRequired = true },
            new() { Id = 7, ServiceId = 1, Label = "Age", FieldType = "number", IsRequired = true },
            new() {
                Id = 8, ServiceId = 1, Label = "Sex", FieldType = "select", IsRequired = true,
                Options = new() { "Male", "Female" }
            },
            new() {
                Id = 9, ServiceId = 1, Label = "Civil Status", FieldType = "select", IsRequired = true,
                Options = new() { "Single", "Married", "Widowed", "Separated" }
            },
            new() { Id = 10, ServiceId = 1, Label = "Religion", FieldType = "text", IsRequired = false },

            new() { Id = 11, ServiceId = 1, Label = "House # / Street", FieldType = "text", IsRequired = true },
            new() { Id = 12, ServiceId = 1, Label = "Barangay", FieldType = "text", IsRequired = true },
            new() { Id = 13, ServiceId = 1, Label = "City", FieldType = "text", IsRequired = true },
            new() { Id = 14, ServiceId = 1, Label = "Province", FieldType = "text", IsRequired = true },
            new() { Id = 15, ServiceId = 1, Label = "Length of Stay (Years)", FieldType = "number", IsRequired = true },
            new() { Id = 16, ServiceId = 1, Label = "Length of Stay (Months)", FieldType = "number", IsRequired = true },

            new() { Id = 17, ServiceId = 1, Label = "Contact Number", FieldType = "text", IsRequired = true },
            new() { Id = 18, ServiceId = 1, Label = "Email Address", FieldType = "email", IsRequired = false }
        }
    },

    // =========================================================
    // 2️⃣ BARANGAY CERTIFICATE
    // =========================================================
    {
        2,
        new List<ServiceRequirementVM>
        {
            new() {
                Id = 101,
                ServiceId = 2,
                Label = "Purpose",
                FieldType = "select",
                IsRequired = true,
                Options = new()
                {
                    "Maynilad",
                    "Meralco",
                    "Senior ID",
                    "Good Moral",
                    "No Derogatory Record"
                }
            },

            new() { Id = 102, ServiceId = 2, Label = "Last Name", FieldType = "text", IsRequired = true },
            new() { Id = 103, ServiceId = 2, Label = "First Name", FieldType = "text", IsRequired = true },
            new() { Id = 104, ServiceId = 2, Label = "Middle Name", FieldType = "text", IsRequired = false },
            new() { Id = 105, ServiceId = 2, Label = "Suffix", FieldType = "text", IsRequired = false },

            new() { Id = 106, ServiceId = 2, Label = "Date of Birth", FieldType = "date", IsRequired = true },
            new() { Id = 107, ServiceId = 2, Label = "Age", FieldType = "number", IsRequired = true },
            new() { Id = 108, ServiceId = 2, Label = "Sex", FieldType = "select", IsRequired = true,
                Options = new() { "Male", "Female" } },
            new() { Id = 109, ServiceId = 2, Label = "Civil Status", FieldType = "select", IsRequired = true,
                Options = new() { "Single", "Married", "Widowed", "Separated" } },
            new() { Id = 110, ServiceId = 2, Label = "Religion", FieldType = "text", IsRequired = false },

            new() { Id = 111, ServiceId = 2, Label = "House # / Street", FieldType = "text", IsRequired = true },
            new() { Id = 112, ServiceId = 2, Label = "Barangay", FieldType = "text", IsRequired = true },
            new() { Id = 113, ServiceId = 2, Label = "City", FieldType = "text", IsRequired = true },
            new() { Id = 114, ServiceId = 2, Label = "Province", FieldType = "text", IsRequired = true },
            new() { Id = 115, ServiceId = 2, Label = "Length of Stay (Years)", FieldType = "number", IsRequired = true },
            new() { Id = 116, ServiceId = 2, Label = "Length of Stay (Months)", FieldType = "number", IsRequired = true },

            new() { Id = 117, ServiceId = 2, Label = "Contact Number", FieldType = "text", IsRequired = true },
            new() { Id = 118, ServiceId = 2, Label = "Email Address", FieldType = "email", IsRequired = false }
        }
    },

    // =========================================================
    // 3️⃣ CEDULA
    // =========================================================
    {
        3,
        new List<ServiceRequirementVM>
        {
            new() { Id = 201, ServiceId = 3, Label = "Last Name", FieldType = "text", IsRequired = true },
            new() { Id = 202, ServiceId = 3, Label = "First Name", FieldType = "text", IsRequired = true },
            new() { Id = 203, ServiceId = 3, Label = "Middle Name", FieldType = "text", IsRequired = false },
            new() { Id = 204, ServiceId = 3, Label = "Suffix", FieldType = "text", IsRequired = false },

            new() { Id = 205, ServiceId = 3, Label = "Date of Birth", FieldType = "date", IsRequired = true },
            new() { Id = 206, ServiceId = 3, Label = "Age", FieldType = "number", IsRequired = true },
            new() { Id = 207, ServiceId = 3, Label = "Sex", FieldType = "select", IsRequired = true,
                Options = new() { "Male", "Female" } },
            new() { Id = 208, ServiceId = 3, Label = "Civil Status", FieldType = "select", IsRequired = true,
                Options = new() { "Single", "Married", "Widowed", "Separated" } },
            new() { Id = 209, ServiceId = 3, Label = "Religion", FieldType = "text", IsRequired = false },

            new() { Id = 210, ServiceId = 3, Label = "House # / Street", FieldType = "text", IsRequired = true },
            new() { Id = 211, ServiceId = 3, Label = "Barangay", FieldType = "text", IsRequired = true },
            new() { Id = 212, ServiceId = 3, Label = "City", FieldType = "text", IsRequired = true },
            new() { Id = 213, ServiceId = 3, Label = "Province", FieldType = "text", IsRequired = true },
            new() { Id = 214, ServiceId = 3, Label = "Length of Stay (Years)", FieldType = "number", IsRequired = true },
            new() { Id = 215, ServiceId = 3, Label = "Length of Stay (Months)", FieldType = "number", IsRequired = true },

            new() { Id = 216, ServiceId = 3, Label = "Contact Number", FieldType = "text", IsRequired = true },
            new() { Id = 217, ServiceId = 3, Label = "Email Address", FieldType = "email", IsRequired = false }
        }
    },

    // =========================================================
    // 4️⃣ BARANGAY ID
    // =========================================================
    {
        4,
        new List<ServiceRequirementVM>
        {
            new() { Id = 301, ServiceId = 4, Label = "Last Name", FieldType = "text", IsRequired = true },
            new() { Id = 302, ServiceId = 4, Label = "First Name", FieldType = "text", IsRequired = true },
            new() { Id = 303, ServiceId = 4, Label = "Middle Name", FieldType = "text", IsRequired = false },
            new() { Id = 304, ServiceId = 4, Label = "Suffix", FieldType = "text", IsRequired = false },

            new() { Id = 305, ServiceId = 4, Label = "Date of Birth", FieldType = "date", IsRequired = true },
            new() { Id = 306, ServiceId = 4, Label = "Age", FieldType = "number", IsRequired = true },
            new() { Id = 307, ServiceId = 4, Label = "Sex", FieldType = "select", IsRequired = true,
                Options = new() { "Male", "Female" } },
            new() { Id = 308, ServiceId = 4, Label = "Civil Status", FieldType = "select", IsRequired = true,
                Options = new() { "Single", "Married", "Widowed", "Separated" } },
            new() { Id = 309, ServiceId = 4, Label = "Religion", FieldType = "text", IsRequired = false },

            new() { Id = 310, ServiceId = 4, Label = "House # / Street", FieldType = "text", IsRequired = true },
            new() { Id = 311, ServiceId = 4, Label = "Barangay", FieldType = "text", IsRequired = true },
            new() { Id = 312, ServiceId = 4, Label = "City", FieldType = "text", IsRequired = true },
            new() { Id = 313, ServiceId = 4, Label = "Province", FieldType = "text", IsRequired = true },
            new() { Id = 314, ServiceId = 4, Label = "Length of Stay (Years)", FieldType = "number", IsRequired = true },
            new() { Id = 315, ServiceId = 4, Label = "Length of Stay (Months)", FieldType = "number", IsRequired = true },

            new() { Id = 316, ServiceId = 4, Label = "Contact Number", FieldType = "text", IsRequired = true },
            new() { Id = 317, ServiceId = 4, Label = "Email Address", FieldType = "email", IsRequired = false },
            new() { Id = 318, ServiceId = 4, Label = "Upload Picture", FieldType = "file", IsRequired = true }
        }
    }
};


        public IActionResult EditServiceRequirements(int id)
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
                return RedirectToAction("Login");

            var requirements = ServiceRequirements.ContainsKey(id)
                ? ServiceRequirements[id]
                : new List<ServiceRequirementVM>();

            ViewBag.ServiceId = id;
            return View(requirements);
        }

        [HttpPost]
        public IActionResult SaveServiceRequirements(
            int serviceId,
            List<ServiceRequirementVM> requirements)
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
                return RedirectToAction("Login");

            requirements = requirements.Where(r => !r.IsDeleted).ToList();

            int nextId = ServiceRequirements.ContainsKey(serviceId)
                ? ServiceRequirements[serviceId].Max(r => r.Id) + 1
                : 1;

            foreach (var r in requirements.Where(r => r.Id == 0))
            {
                r.Id = nextId++;
                r.ServiceId = serviceId;
            }

            ServiceRequirements[serviceId] = requirements;
            return RedirectToAction("BarangayServices");
        }


        private List<ServiceRequestVM> GetRequests()
        {
            var requests = HttpContext.Session.GetObject<List<ServiceRequestVM>>("Requests");
            if (requests != null) return requests;

            requests = SeedRequests();
            SaveRequests(requests);
            return requests;
        }

        private void SaveRequests(List<ServiceRequestVM> requests)
            => HttpContext.Session.SetObject("Requests", requests);

        private List<ServiceRequestVM> SeedRequests() => new()
        {
            new() { Id=1, FirstName="Juan", LastName="Dela Cruz", ServiceType="Barangay Clearance", Status="Pending" },
            new() { Id=2, FirstName="Maria", LastName="Santos", ServiceType="Cedula", Status="Approved" },
            new() { Id=3, FirstName="Pedro", LastName="Penduko", ServiceType="Barangay Certificate", Status="Ready for Claim" }
        };



        [HttpPost]
        public IActionResult ToggleService(int id)
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
                return RedirectToAction("Login");

            var service = Services.FirstOrDefault(s => s.Id == id);

            if (service != null)
            {
                service.IsEnabled = !service.IsEnabled;
            }

            return RedirectToAction("BarangayServices");
        }

        [HttpGet]
        public IActionResult AddService()
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
                return RedirectToAction("Login");

            return View();
        }

        [HttpPost]
        public IActionResult AddService(ServiceVM model)
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
                return RedirectToAction("Login");

            if (!ModelState.IsValid)
                return View(model);

            model.Id = Services.Max(s => s.Id) + 1;
            model.IsEnabled = true; // enabled by default

            Services.Add(model);

            return RedirectToAction("BarangayServices");
        }

        public IActionResult AdminProfile()
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
                return RedirectToAction("Login");

            ViewBag.BarangayName = "Barangay Malanday"; // fake for now
            ViewBag.AdminName = "Administrator";
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(string currentPassword, string newPassword)
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
                return RedirectToAction("Login");

            // FAKE LOGIC FOR NOW
            // Later connect to DB + hashing

            TempData["Success"] = "Password successfully updated.";
            return RedirectToAction("AdminProfile");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // FIXED ADMIN ACCOUNT
            if (username == "admin" && password == "admin123")
            {
                HttpContext.Session.SetString("AdminLoggedIn", "true");
                return RedirectToAction("Dashboard");
            }

            ViewBag.Error = "Invalid admin credentials";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }






    }
}





