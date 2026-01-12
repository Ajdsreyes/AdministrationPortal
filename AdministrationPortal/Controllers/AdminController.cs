using Microsoft.AspNetCore.Mvc;
using AdministrationPortal.Models.ViewModels;
using AdministrationPortal.Helpers;


namespace BarangayAdminPortal.Controllers
{
    public class AdminController : Controller
    {

        private static List<UserVM> FakeUsers = new List<UserVM>
{
    new UserVM
    {
        Id = 1,
        LastName = "Dela Cruz",
        FirstName = "Juan",
        MiddleName = "S",
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


        // =======================
        // DASHBOARD (FAKE DATA)
        // =======================
        public IActionResult Dashboard()
        {
            var model = new AdminDashboardVM
            {
                TotalUsers = 12000,
                TotalRequests = 50,
                ApprovedRequests = 73,
                RejectedRequests = 2,
                ReadyToClaim = 105
            };

            return View(model);
        }

        // =======================
        // MANAGE USERS (FAKE DATA)
        // =======================
        public IActionResult ManageUsers()
        {
            return View(FakeUsers);
        }

        public IActionResult ViewUser(int id)
        {
            var user = FakeUsers.FirstOrDefault(u => u.Id == id);
            return View(user);
        }

        // GET: Edit User
        public IActionResult EditUser(int id)
        {
            var user = FakeUsers.FirstOrDefault(u => u.Id == id);
            return View(user);
        }

        // POST: Edit User
        [HttpPost]
        public IActionResult EditUser(UserVM model)
        {
            var user = FakeUsers.FirstOrDefault(u => u.Id == model.Id);

            if (user != null)
            {
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
                user.IsVoter = model.IsVoter;
            }

            return RedirectToAction("ViewUser", new { id = model.Id });
        }



        public IActionResult Deactivate(int id)
        {
            var user = FakeUsers.FirstOrDefault(u => u.Id == id);
            if (user != null)
                user.IsActive = false;

            return RedirectToAction("ManageUsers");
        }

        public IActionResult Reactivate(int id)
        {
            var user = FakeUsers.FirstOrDefault(u => u.Id == id);
            if (user != null)
                user.IsActive = true;

            return RedirectToAction("ManageUsers");
        }

        // =======================
        // BARANGAY SERVICES
        // =======================
        public IActionResult BarangayServices()
        {
            return View(FakeServices());
        }

        public IActionResult ServiceRequests()
        {
            // Optional title for page header
            ViewBag.ServiceName = "Barangay Service Requests";

            // Always return a LIST
            return View(FakeRequests());
        }

        [HttpPost]
        public IActionResult UpdateClaimSchedule(
    int id,
    DateTime? dateToClaim,
    TimeSpan? timeToClaim,
    string status)
        {
            var request = Requests.FirstOrDefault(r => r.Id == id);

            if (request == null)
                return NotFound();

            request.Status = status;

            if (status == "Approved" || status == "Ready for Claim")
            {
                request.DateToClaim = dateToClaim;
                request.TimeToClaim = timeToClaim;
            }
            else
            {
                request.DateToClaim = null;
                request.TimeToClaim = null;
            }

            HttpContext.Session.SetObject("Requests", Requests);

            return RedirectToAction("ViewServiceRequest", new { id });
        }


        private List<ServiceRequestVM> Requests
        {
            get
            {
                var requests = HttpContext.Session.GetObject<List<ServiceRequestVM>>("Requests");

                if (requests == null)
                {
                    requests = FakeRequests(); //  fake data
                    HttpContext.Session.SetObject("Requests", requests);
                }

                return requests;
            }
        }




        public IActionResult ViewServiceRequest(int id)
        {
            var request = FakeServiceRequests()
                .FirstOrDefault(r => r.Id == id);

            if (request == null)
                return NotFound();

            ViewBag.ServiceName = request.ServiceType;

            return View(request); // ✅ sends ONE ServiceRequestVM
        }

        private List<ServiceRequestVM> FakeServiceRequests()
        {
            return new List<ServiceRequestVM>
    {
        new ServiceRequestVM
        {
            Id = 1,
            ServiceType = "Barangay Clearance",
            FirstName = "Juan",
            LastName = "Dela Cruz",
            Email = "juan@email.com",
            Status = "Pending",
            DateRequested = DateTime.Today,
            UploadedPhotoUrl = "/images/avatar1.png",
            Purpose = "Local Employment",
        },

        new ServiceRequestVM
        {
            Id = 2,
            ServiceType = "Barangay Clearance",
            FirstName = "Maria",
            LastName = "Santos",
            Email = "maria@email.com",
            Status = "Approved",
            DateRequested = DateTime.Today.AddDays(-1),
            UploadedPhotoUrl = "/images/avatar2.png",
            Purpose = "Scholarship",
        }
    };
        }


        // Enable / Disable
        public IActionResult ToggleService(int id)
        {
            return RedirectToAction("BarangayServices");
        }

        private List<ServiceVM> FakeServices()
        {
            return new List<ServiceVM>
    {
        new ServiceVM { Id = 1, ServiceName = "Barangay Clearance", Icon="bi-file-earmark-text", IsEnabled=true },
        new ServiceVM { Id = 2, ServiceName = "Barangay Certificate", Icon="bi-file-earmark", IsEnabled=true },
        new ServiceVM { Id = 3, ServiceName = "Cedula", Icon="bi-receipt", IsEnabled=true },
        new ServiceVM { Id = 4, ServiceName = "Barangay ID", Icon="bi-person-badge", IsEnabled=false }
    };
        }

        private List<ServiceRequestVM> FakeRequests()
        {
            return new List<ServiceRequestVM>
    {
        new ServiceRequestVM
        {
            Id = 1,
            FirstName = "Juan",
            MiddleName = "",
            LastName = "Dela Cruz",
            Suffix = "",
            DateRequested = DateTime.Today,
            Status = "Pending"
        },
        new ServiceRequestVM
        {
             Id = 2,
             FirstName = "Maria",
             MiddleName = "",
             LastName = "Santos",
             Suffix = "",
             DateRequested = DateTime.Today.AddDays(-1),
             Status = "Approved"
                }

            };
        }
    }
}




/*using AdministrationPortal.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BarangayAdminPortal.Controllers
{
    public class AdminController : Controller
    {
        private readonly HttpClient _http;

        public AdminController(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("BarangayAPI");
        }

        public async Task<IActionResult> Dashboard()
        {
            var data = await _http.GetFromJsonAsync<AdminDashboardVM>(
                "/api/Dashboard"
            );

            return View(data);
        }

        public async Task<IActionResult> ManageUsers()
        {
            var users = await _http.GetFromJsonAsync<List<UserVM>>(
                "/api/admin/users"
            );

            return View(users);
        }

        public async Task<IActionResult> Deactivate(int id)
        {
            await _http.PostAsync($"/api/admin/users/deactivate/{id}", null);
            return RedirectToAction("ManageUsers");
        }

        public async Task<IActionResult> Reactivate(int id)
        {
            await _http.PostAsync($"/api/admin/users/reactivate/{id}", null);
            return RedirectToAction("ManageUsers");
        }


        public IActionResult Services()
        {
            return View();
        }

        public IActionResult Settings()
        {
            return View();
        }
    }
} */
