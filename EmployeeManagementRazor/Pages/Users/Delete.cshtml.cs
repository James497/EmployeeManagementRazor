using EmployeeManagementRazor.Models;
using EmployeeManagementRazor.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace EmployeeManagementRazor.Pages.Users
{
    [Authorize(Roles = "Admin,Manager")]
    public class DeleteModel : PageModel
    {
        private readonly UserManager<IdentityUser> userManager;

        public DeleteModel(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        [BindProperty]
        public DeleteUserViewModel userModel { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return RedirectToPage("/NotFound");
            }
            userModel = new DeleteUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName!,
                Email = user.Email!
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await userManager.FindByIdAsync(userModel.Id);
            if(user == null)
            {
                return RedirectToPage("/NotFound");
            }
            var result = await userManager.DeleteAsync(user);

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
    public class DeleteUserViewModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }
    }
}
