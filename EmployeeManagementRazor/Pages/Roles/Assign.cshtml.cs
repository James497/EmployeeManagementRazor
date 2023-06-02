using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagementRazor.Pages.Roles
{
    public class AssignModel : PageModel
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;

        public AssignModel(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        [BindProperty(SupportsGet = true)]
        public string RoleId { get; set; }
        [BindProperty]
        public List<UserRoleViewModel> UsersInRole { get; set; }
        public async Task OnGetAsync()
        {
            var role = await roleManager.FindByIdAsync(RoleId);
            if (role == null)
            {
                RedirectToPage("/NotFound");
                return;
            }
            UsersInRole = new();
            foreach(var user in userManager.Users.ToList())
            {
                var userRoleViewModel = new UserRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };
                if(await userManager.IsInRoleAsync(user, role.Name!))
                {
                    userRoleViewModel.IsSelected = true;
                }
                UsersInRole.Add(userRoleViewModel);
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var role = await roleManager.FindByIdAsync(RoleId);

            if (role == null)
            {
                return RedirectToPage("/NotFound");
            }

            for (int i = 0; i < UsersInRole.Count; i++)
            {
                var user = await userManager.FindByIdAsync(UsersInRole[i].UserId);

                IdentityResult result = null;

                if (UsersInRole[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!UsersInRole[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }
            }

            return RedirectToPage("Edit", new { Id = RoleId });
        }
    }
    public class UserRoleViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool IsSelected { get; set; }
    }
}
