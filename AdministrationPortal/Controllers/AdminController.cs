using Microsoft.AspNetCore.Mvc;
using AdministrationPortal.Models;
using AdministrationPortal.Models.ViewModels;
using AdministrationPortal.Data;
using AdministrationPortal.Helpers;
using Microsoft.EntityFrameworkCore;

namespace AdministrationPortal.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ======================================================
        // LOGIN / AUTHENTICATION
        // ======================================================
        [HttpGet]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var admin = _context.Admins
                .FirstOrDefault(a => a.Username == username && a.AdminPassword == password);

            if (admin != null)
            {
                HttpContext.Session.SetObject("ActiveAdmin", admin);
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

        // ======================================================
        // DASHBOARD
        // ======================================================
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
                return RedirectToAction("Login");

            var model = new AdminDashboardVM
            {
                TotalUsers = _context.Users.Count(),
                TotalRequests = _context.ServiceRequests.Count(),
                ApprovedRequests = _context.ServiceRequests.Count(r => r.StatusId == 2),
                RejectedRequests = _context.ServiceRequests.Count(r => r.StatusId == 3),
                ReadyToClaim = _context.ServiceRequests.Count(r => r.StatusId == 4)
            };

            return View(model);
        }

        // ======================================================
        // MANAGE USERS (RESIDENTS)
        // ======================================================
        public IActionResult ManageUsers()
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true") return RedirectToAction("Login");

            var users = _context.Users.Select(u => new UserVM
            {
                UserId = u.UserId,
                FirstName = u.FirstName,
                LastName = u.LastName,
                MiddleName = u.MiddleName,
                Suffix = u.Suffix,
                HouseNoStreet = u.HouseNoStreet,
                Barangay = u.Barangay,
                City = u.City,
                Province = u.Province,
                ContactNo = u.ContactNo,
                IsActive = u.IsActive,
                DateOfBirth = u.DateOfBirth ?? DateTime.MinValue,
                PhotoUrl = u.UploadPath
            }).ToList();

            return View(users);
        }

        [HttpGet]
        public IActionResult EditUser(int id)
        {
            var u = _context.Users.Find(id);
            if (u == null) return NotFound();

            var vm = new UserVM
            {
                UserId = u.UserId,
                FirstName = u.FirstName,
                LastName = u.LastName,
                MiddleName = u.MiddleName,
                Suffix = u.Suffix,
                DateOfBirth = u.DateOfBirth ?? DateTime.MinValue,
                Sex = u.Sex,
                CivilStatus = u.CivilStatus,
                Religion = u.Religion,
                HouseNoStreet = u.HouseNoStreet,
                Barangay = u.Barangay,
                City = u.City,
                Province = u.Province,
                StayYears = u.StayYears,
                StayMonths = u.StayMonths,
                ContactNo = u.ContactNo,
                Email = u.Email,
                IsVoter = u.IsVoter,
                IsActive = u.IsActive
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult EditUser(UserVM vm)
        {
            var u = _context.Users.Find(vm.UserId);
            if (u == null) return NotFound();

            u.FirstName = vm.FirstName;
            u.LastName = vm.LastName;
            u.MiddleName = vm.MiddleName;
            u.Suffix = vm.Suffix;
            u.DateOfBirth = vm.DateOfBirth;
            u.Sex = vm.Sex;
            u.CivilStatus = vm.CivilStatus;
            u.Religion = vm.Religion;
            u.HouseNoStreet = vm.HouseNoStreet;
            u.Barangay = vm.Barangay;
            u.City = vm.City;
            u.Province = vm.Province;
            u.StayYears = vm.StayYears;
            u.StayMonths = vm.StayMonths;
            u.ContactNo = vm.ContactNo;
            u.Email = vm.Email;
            u.IsVoter = vm.IsVoter;

            _context.SaveChanges();
            TempData["Success"] = "Resident profile updated.";
            return RedirectToAction("ManageUsers");
        }

        public IActionResult ViewUser(int id)
        {
            var u = _context.Users.Find(id);
            if (u == null) return NotFound();

            var vm = new UserVM
            {
                UserId = u.UserId,
                FirstName = u.FirstName,
                LastName = u.LastName,
                MiddleName = u.MiddleName,
                Suffix = u.Suffix,
                DateOfBirth = u.DateOfBirth ?? DateTime.MinValue,
                Sex = u.Sex,
                CivilStatus = u.CivilStatus,
                Religion = u.Religion,
                HouseNoStreet = u.HouseNoStreet,
                Barangay = u.Barangay,
                City = u.City,
                Province = u.Province,
                StayYears = u.StayYears,
                StayMonths = u.StayMonths,
                ContactNo = u.ContactNo,
                Email = u.Email,
                IsVoter = u.IsVoter,
                IsActive = u.IsActive,
                PhotoUrl = u.UploadPath
            };
            return View(vm);
        }

        public IActionResult Deactivate(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null) { user.IsActive = false; _context.SaveChanges(); }
            return RedirectToAction("ManageUsers");
        }

        public IActionResult Reactivate(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null) { user.IsActive = true; _context.SaveChanges(); }
            return RedirectToAction("ManageUsers");
        }

        // ======================================================
        // SERVICES & REQUIREMENTS
        // ======================================================
        [HttpGet]
        public IActionResult BarangayServices()
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true") return RedirectToAction("Login");

            var services = _context.Services.Select(s => new ServiceVM
            {
                ServiceId = s.ServiceId,
                ServiceName = s.ServiceName,
                IsEnabled = s.IsEnabled,
                Pending = _context.ServiceRequests.Count(r => r.ServiceId == s.ServiceId && r.StatusId == 1),
                Approved = _context.ServiceRequests.Count(r => r.ServiceId == s.ServiceId && r.StatusId == 2),
                Ready = _context.ServiceRequests.Count(r => r.ServiceId == s.ServiceId && r.StatusId == 4)
            }).ToList();

            return View(services);
        }

        [HttpPost]
        public IActionResult ToggleService(int id)
        {
            var service = _context.Services.Find(id);
            if (service != null) { service.IsEnabled = !service.IsEnabled; _context.SaveChanges(); }
            return RedirectToAction("BarangayServices");
        }

        // This loads the "Login-style" page
        [HttpGet]
        public IActionResult AddService()
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true") return RedirectToAction("Login");
            return View(new ServiceVM());
        }

        // This processes the creation and prevents duplicates
        [HttpPost]
        public IActionResult AddService(ServiceVM vm)
        {
            var exists = _context.Services.Any(s => s.ServiceName.ToLower() == vm.ServiceName.ToLower());

            if (exists)
            {
                TempData["Error"] = "This service already exists.";
                return RedirectToAction("AddService"); // Stay on page to show error
            }

            var service = new Service { ServiceName = vm.ServiceName, IsEnabled = true };
            _context.Services.Add(service);
            _context.SaveChanges();

            return RedirectToAction("BarangayServices");
        }

        [HttpGet]
        public IActionResult EditServiceRequirements(int id)
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true") return RedirectToAction("Login");

            var purposes = _context.ServicePurposes
                .Where(p => p.ServiceId == id && (p.IsEnabled == true || p.IsEnabled == null)).ToList();

            var vmList = purposes.Select(p => new ServiceRequirementVM
            {
                PurposeId = p.PurposeId,
                ServiceId = p.ServiceId,
                PurposeName = p.PurposeName ?? "New Requirement",
                // Use ?? to provide a default value if the database column is null
                FieldType = p.FieldType ?? "Text",
                IsRequired = p.IsRequired,
                Options = !string.IsNullOrEmpty(p.Options)
                            ? p.Options.Split(',').ToList()
                            : new List<string>()
            }).ToList();

            ViewBag.ServiceId = id;
            return View(vmList);
        }

        [HttpPost]
        public IActionResult SaveServiceRequirements(int serviceId, List<ServiceRequirementVM> requirements)
        {
            foreach (var vm in requirements)
            {
                var existingPurpose = _context.ServicePurposes.Find(vm.PurposeId);

                if (vm.IsDeleted)
                {
                    if (existingPurpose != null)
                    {
                        existingPurpose.IsEnabled = false;
                    }
                }
                else
                {
                    if (existingPurpose != null)
                    {
                        // UPDATE
                        existingPurpose.PurposeName = vm.PurposeName;
                        existingPurpose.FieldType = vm.FieldType;
                        existingPurpose.IsRequired = vm.IsRequired;
                        existingPurpose.IsEnabled = true;
                        existingPurpose.Options = vm.Options != null ? string.Join(",", vm.Options) : null;
                        _context.ServicePurposes.Update(existingPurpose);
                    }
                    else
                    {
                        // INSERT
                        var newPurpose = new ServicePurpose
                        {
                            ServiceId = serviceId,
                            PurposeName = vm.PurposeName,
                            FieldType = vm.FieldType,
                            IsRequired = vm.IsRequired,
                            IsEnabled = true,
                            Options = vm.Options != null ? string.Join(",", vm.Options) : null
                        };
                        _context.ServicePurposes.Add(newPurpose);
                    }
                }
            }
            _context.SaveChanges();
            return RedirectToAction("BarangayServices");
        }

        // ======================================================
        // SERVICE REQUESTS
        // ======================================================
        public IActionResult ServiceRequests()
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true") return RedirectToAction("Login");

            var requests = _context.ServiceRequests
                .Include(r => r.User).Include(r => r.Service).Include(r => r.Status)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new ServiceRequestVM
                {
                    RequestId = r.RequestId,
                    FirstName = r.User.FirstName,
                    LastName = r.User.LastName,
                    ServiceName = r.Service.ServiceName,
                    StatusName = r.Status.StatusName,
                    CreatedAt = r.CreatedAt,
                    DateToClaim = r.DateToClaim,
                    TimeToClaim = r.TimeToClaim,
                    UploadPath = r.UploadPath
                }).ToList();

            return View(requests);
        }

        public IActionResult ViewServiceRequest(int id)
        {
            var request = _context.ServiceRequests
                .Include(r => r.User).Include(r => r.Status).Include(r => r.Service)
                .FirstOrDefault(r => r.RequestId == id);

            if (request == null) return NotFound();

            return View(new ServiceRequestVM
            {
                RequestId = request.RequestId,
                ServiceName = request.Service?.ServiceName ?? "Unknown",
                CreatedAt = request.CreatedAt,
                StatusName = request.Status?.StatusName ?? "Pending",
                FirstName = request.User?.FirstName ?? "",
                LastName = request.User?.LastName ?? "",
                HouseNoStreet = request.User?.HouseNoStreet ?? "",
                Barangay = request.User?.Barangay ?? "",
                City = request.User?.City ?? "",
                Province = request.User?.Province ?? "",
                ContactNo = request.User?.ContactNo ?? "",
                Email = request.User?.Email ?? "",
                DateToClaim = request.DateToClaim,
                TimeToClaim = request.TimeToClaim,
                UploadPath = request.UploadPath,
                Sex = request.User?.Sex,
                CivilStatus = request.User?.CivilStatus,
                Religion = request.User?.Religion,
                DateOfBirth = request.User?.DateOfBirth ?? DateTime.MinValue,
                StayYears = request.User?.StayYears ?? 0,
                StayMonths = request.User?.StayMonths ?? 0,
            });
        }

        [HttpPost]
        public IActionResult UpdateClaimSchedule(int id, DateTime? DateToClaim, TimeSpan? TimeToClaim, string Status)
        {
            var request = _context.ServiceRequests.Find(id);
            if (request == null) return NotFound();

            request.DateToClaim = DateToClaim;
            request.TimeToClaim = TimeToClaim;
            request.StatusId = Status switch
            {
                "Pending" => 1,
                "Approved" => 2,
                "Rejected" => 3,
                "Ready for Claim" => 4,
                "Released" => 5,
                _ => request.StatusId
            };

            _context.SaveChanges();
            return RedirectToAction("ViewServiceRequest", new { id = id });
        }

        // ======================================================
        // ADMIN PROFILE
        // ======================================================
        public IActionResult AdminProfile()
        {
            var admin = HttpContext.Session.GetObject<Admin>("ActiveAdmin");
            if (admin == null) return RedirectToAction("Login");
            return View(admin);
        }

        [HttpPost]
        public IActionResult ChangePassword(string currentPassword, string newPassword)
        {
            var activeAdmin = HttpContext.Session.GetObject<Admin>("ActiveAdmin");
            var admin = _context.Admins.Find(activeAdmin?.AdminId);

            if (admin != null && admin.AdminPassword == currentPassword)
            {
                admin.AdminPassword = newPassword;
                _context.SaveChanges();
                TempData["Success"] = "Password updated successfully.";
            }
            else { TempData["Error"] = "Current password incorrect."; }

            return RedirectToAction("AdminProfile");
        }
    }
}