using EmployeeManagementRazor.Services.Data;
using EmployeeManagementRazor.Services.Interfaces;
using EmployeeManagementRazor.Services.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementRazor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
                options.AppendTrailingSlash = true;
            });
            builder.Services.AddDbContextPool<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("EmployeeDBConnectionRazor"));
                
            });
            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();
            builder.Services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.AddPolicy("DeleteRolePolicy", policy => policy.RequireClaim("Delete Role"));
            });
            builder.Services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();
            builder.Logging.AddSentry(o =>
            {
                o.Dsn = builder.Configuration.GetConnectionString("Dsn");
                o.Debug = true;
                o.TracesSampleRate = 1.0;
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/NotFound");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.Logger.LogError("CS is :" + builder.Configuration.GetConnectionString("EmployeeDBConnectionRazor"));
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSentryTracing();
            app.MapRazorPages();

            app.Run();
        }
    }
}