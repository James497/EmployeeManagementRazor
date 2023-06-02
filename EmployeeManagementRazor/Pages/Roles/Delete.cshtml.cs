using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace EmployeeManagementRazor.Pages.Roles
{
    [Authorize(Roles = "Admin,Manager")]
    [Authorize(Policy = "DeleteRolePolicy")]
    public class DeleteModel : PageModel
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public DeleteModel(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        [BindProperty]
        public DeleteRoleViewModel roleModel { get; set; }

        public async Task<IActionResult> OnGetAsync(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                return RedirectToPage("/NotFound");
            }
            roleModel = new DeleteRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name!
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var role = await roleManager.FindByIdAsync(roleModel.Id);
            if (role == null)
            {
                return RedirectToPage("/NotFound");
            }
            var result = await roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                return RedirectToPage("/Roles/Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return Page();
        }
    }
    public class DeleteRoleViewModel
    {
        public string Id { get; set; }

        public string RoleName { get; set; }
    }
}
