using AdministrationPortal.Data; // Ensure this matches your project name
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. DATABASE CONNECTION
// This pulls the connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. ADD MVC SERVICES
builder.Services.AddControllersWithViews();

// 3. SESSION & CONTEXT ACCESSOR
// AddHttpContextAccessor allows your Helpers/SessionExtensions to work properly
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Admin stays logged in for 30 mins of inactivity
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = ".iAssist.Session"; // Custom name for your Barangay Portal cookie
});

var app = builder.Build();

// 4. CONFIGURE THE HTTP REQUEST PIPELINE
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();



app.UseRouting();

// THE ORDER MATTERS HERE:
// Routing -> Session -> Authorization
app.UseSession();
app.UseAuthorization();

// 5. DEFAULT ROUTE
// Set to Admin/Login so the portal opens to the login page immediately
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Admin}/{action=Login}/{id?}");

app.Run();