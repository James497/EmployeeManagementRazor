using EmployeeManagementRazor.Models;
using EmployeeManagementRazor.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementRazor.ViewComponents
{
    public class HeadCountViewComponent : ViewComponent
    {
        private readonly IEmployeeRepository employeeRepository;

        public HeadCountViewComponent(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        public IViewComponentResult Invoke(Dept? department = null)
        {
            var result = employeeRepository.EmployeeCountByDept(department);
            return View(result);
        }
    }
}
