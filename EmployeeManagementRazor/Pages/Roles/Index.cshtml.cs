using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagementRazor.Pages.Roles
{
    [Authorize(Roles = "Admin,Manager")]
    public class IndexModel : PageModel
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public IEnumerable<IdentityRole> Roles { get; set; }
        public IndexModel(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }
        public void OnGet()
        {
            Roles = roleManager.Roles.ToList();
        }
    }
}
