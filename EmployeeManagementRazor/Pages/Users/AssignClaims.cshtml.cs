using EmployeeManagementRazor.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace EmployeeManagementRazor.Pages.Users
{
    public class AssignClaimsModel : PageModel
    {
        private readonly UserManager<IdentityUser> userManager;

        public AssignClaimsModel(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }
        [BindProperty(SupportsGet = true)]
        public string UserId { get; set; }
        [BindProperty]
        public UserClaimsViewModel userClaimsViewModel { get; set; }
        public async Task OnGetAsync()
        {
            var user = await userManager.FindByIdAsync(UserId);

            if (user == null)
            {
                RedirectToPage("/NotFound");
                return;
            }

            // UserManager service GetClaimsAsync method gets all the current claims of the user
            var existingUserClaims = await userManager.GetClaimsAsync(user);

            userClaimsViewModel = new UserClaimsViewModel
            {
                UserId = UserId
            };

            foreach (Claim claim in ClaimsStore.AllClaims)
            {
                UserClaim userClaim = new UserClaim
                {
                    ClaimType = claim.Type
                };

                // If the user has the claim, set IsSelected property to true, so the checkbox
                // next to the claim is checked on the UI
                if (existingUserClaims.Any(c => c.Type == claim.Type))
                {
                    userClaim.IsSelected = true;
                }

                userClaimsViewModel.Claims.Add(userClaim);
            }

        }
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await userManager.FindByIdAsync(userClaimsViewModel.UserId);

            if (user == null)
            {
                return RedirectToPage("/NotFound");
            }

            // Get all the user existing claims and delete them
            var claims = await userManager.GetClaimsAsync(user);
            var result = await userManager.RemoveClaimsAsync(user, claims);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user from existing claims");
                return Page();
            }

            // Add all the claims that are selected on the UI
            result = await userManager.AddClaimsAsync(user,
                userClaimsViewModel.Claims.Where(c => c.IsSelected).Select(c => new Claim(c.ClaimType, c.ClaimType)));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected claims to user");
                return Page();
            }

            return RedirectToPage("Edit", new { userId = UserId });
        }
    }

    public class UserClaimsViewModel
    {
        public string UserId { get; set; }
        public List<UserClaim> Claims { get; set; } = new List<UserClaim>();
    }
    public class UserClaim
    {
        public string ClaimType { get; set; }
        public bool IsSelected { get; set; }
    }
}
