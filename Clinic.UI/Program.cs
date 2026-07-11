using Clinic.Application;
using Clinic.Application.Contracts.UserContract;
using Clinic.Application.Mapping;
using Clinic.Infrastructure;
using Clinic.Infrastructure.Context;
using Clinic.Infrastructure.UserModels;
using Clinic.UI.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

namespace UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // =========================
            // Serilog (Safe Config)
            // =========================
            var isEfMigration = AppDomain.CurrentDomain
                .GetAssemblies()
                .Any(a => a.FullName.Contains("EntityFrameworkCore.Design"));

            if (isEfMigration)
            {
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Warning()
                    .WriteTo.Console()
                    .CreateLogger();
            }
            else
            {
                // وقت تشغيل التطبيق العادي
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Warning()  // ← هنا
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Error)  // ← وهنا
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("Application", "DreamSystem")
                    .WriteTo.Console()
                    .WriteTo.MSSqlServer(
                        connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
                        tableName: "Logs",
                        autoCreateSqlTable: true)
                    .CreateLogger();
            }

            builder.Host.UseSerilog();

            // =========================
            // Services
            // =========================
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ClinicContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/login";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                    options.SlidingExpiration = true;
                    options.Cookie.IsEssential = true;
                    options.ExpireTimeSpan = TimeSpan.FromDays(1);
                });

            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ClinicContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddAuthorization();

            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

            builder.Services.AddModuleInfrastructureDependencies(builder.Configuration);
            builder.Services.AddModuleApplicationDependencies(builder.Configuration);

            builder.Services.AddScoped<IUserService, UserService>();

            var app = builder.Build();

            // =========================
            // Middleware
            // =========================
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "admin",
                pattern: "{area:Exists}/{controller=Patient}/{action=Index}/{id?}"
            );

            app.MapControllerRoute(
                name: "doctor",
                pattern: "{area:Exists}/{controller=Doctor}/{action=Index}/{id?}"
            );

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Appointment}/{action=Today}/{id?}"
            );

            // =========================
            // Safe Seeding
            // =========================
            if (app.Environment.IsDevelopment())
            {
                using var scope = app.Services.CreateScope();
                var services = scope.ServiceProvider;

                try
                {
                    var userManager = services.GetRequiredService<UserManager<AppUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    string adminPassword = builder.Configuration["AdminSettings:Password"] ?? "Admin@1234";

                    DbSeeder.SeedAdminAsync(userManager, roleManager, adminPassword)
                        .GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Seeding Failed");
                }
            }

            app.Run();
        }
    }
}