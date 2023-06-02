using EmployeeManagementRazor.Models;
using EmployeeManagementRazor.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagementRazor.Pages.Employees
{
    [Authorize(Roles ="Admin,Manager")]
    public class DeleteModel : PageModel
    {
        private readonly IEmployeeRepository employeeRepository;

        public DeleteModel(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        [BindProperty]
        public Employee Employee { get; set; }

        public IActionResult OnGet(int id)
        {
            Employee = employeeRepository.GetEmployee(id);

            if (Employee == null)
            {
                return RedirectToPage("/NotFound");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            Employee deletedEmployee = employeeRepository.Delete(Employee.Id);

            if (deletedEmployee == null)
            {
                return RedirectToPage("/NotFound");
            }

            return RedirectToPage("Index");
        }
    }
}
