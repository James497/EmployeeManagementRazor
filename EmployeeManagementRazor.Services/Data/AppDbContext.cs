using EmployeeManagementRazor.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementRazor.Services.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        public DbSet<Employee> Employees { get; set; }
    }
    
}
