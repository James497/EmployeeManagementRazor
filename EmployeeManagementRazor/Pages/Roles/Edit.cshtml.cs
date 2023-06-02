using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace EmployeeManagementRazor.Pages.Roles
{
    [Authorize(Roles = "Admin,Manager")]
    public class EditModel : PageModel
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;

        public EditModel(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        [BindProperty]
        public RoleModel roleModel { get; set; }
        public async Task OnGet(string? id)
        {
            if(!string.IsNullOrEmpty(id))
            {
                var role = await roleManager.FindByIdAsync(id);
                roleModel = new RoleModel()
                {
                    Id = id,
                    RoleName = role.Name
                };
                roleModel.Users.Clear();
                //Map users
                var users = await userManager.GetUsersInRoleAsync(role.Name!);
                roleModel.Users = users.Select(x => x.UserName!).ToList();
            }
            if (roleModel == null)
            {
                roleModel = new RoleModel();
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                // We just need to specify a unique role name to create a new role
                IdentityRole identityRole = new IdentityRole
                {
                    Name = roleModel.RoleName
                };
                IdentityResult result = default;
                if (string.IsNullOrEmpty(roleModel.Id))
                {
                    // Saves the role in the underlying AspNetRoles table
                    result = await roleManager.CreateAsync(identityRole);
                }
                else
                {
                    var role = await roleManager.FindByIdAsync(roleModel.Id);
                    role.Name = roleModel.RoleName;
                    result = await roleManager.UpdateAsync(role);
                }
                if (result.Succeeded)
                {
                    return RedirectToPage("/Roles/Index");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return Page();
        }
    }
    public class RoleModel
    {
        public string? Id { get; set; }
        [Required]
        public string RoleName { get; set; }
        public List<string> Users { get; set; } = new List<string>();
    }
}
