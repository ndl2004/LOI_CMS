using CMS.Data;
using CMS_Backend.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ======================================
// MVC + API
// ======================================

builder.Services.AddControllersWithViews();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ======================================
// SQL Server
// ======================================

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// ======================================
// Cookie Authentication
// ======================================

builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme
)
.AddCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// ======================================
// CORS cho ReactJS
// ======================================

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins(
                "http://localhost:3000",
                "http://localhost:5173"
            )
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// ======================================
// Email Service
// ======================================

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings")
);

builder.Services.AddScoped<IEmailService, EmailService>();

// ======================================

var app = builder.Build();

// ======================================
// Error Handling
// ======================================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// ======================================
// Swagger
// ======================================

app.UseSwagger();
app.UseSwaggerUI();

// ======================================

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

// ======================================
// CORS
// ======================================

app.UseCors("AllowReactApp");

// ======================================
// Authentication + Authorization
// ======================================

app.UseAuthentication();

app.UseAuthorization();

// ======================================
// MVC Route
// ======================================

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

// ======================================
// API Route
// ======================================

app.MapControllers();

// ======================================

app.Run();