using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementRazor.Pages.Users
{
    [Authorize(Roles = "Admin,Manager")]
    public class EditModel : PageModel
    {
        private readonly UserManager<IdentityUser> userManager;

        public EditModel(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task OnGet(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                RedirectToPage("/NotFound");
                return;
            }

            // GetClaimsAsync retunrs the list of user Claims
            var userClaims = await userManager.GetClaimsAsync(user);
            // GetRolesAsync returns the list of user Roles
            var userRoles = await userManager.GetRolesAsync(user);

            userViewModel = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Claims = userClaims.Select(c => c.Value).ToList(),
                Roles = userRoles.ToList()
            };
        }
        [BindProperty]
        public EditUserViewModel userViewModel { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await userManager.FindByIdAsync(userViewModel.Id);

            if (user == null)
            {
                return RedirectToPage("/NotFound");
            }
            else
            {
                user.Email = userViewModel.Email;
                user.UserName = userViewModel.UserName;

                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToPage("/Users/Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return Page();
            }
        }
    }

    public class EditUserViewModel
    {

        public string Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public List<string> Claims { get; set; } = new();

        public List<string> Roles { get; set; } = new();
    }

}
