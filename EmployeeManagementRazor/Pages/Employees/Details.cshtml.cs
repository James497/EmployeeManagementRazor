using EmployeeManagementRazor.Models;
using EmployeeManagementRazor.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagementRazor.Pages.Employees
{
    public class DetailsModel : PageModel
    {
        public Employee Employee { get; private set; }
        private readonly IEmployeeRepository _employeeRepository;

        public DetailsModel(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        public void OnGet(int id)
        {
            Employee = _employeeRepository.GetEmployee(id);
        }
    }
}