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

        public IViewComponentResult Invoke()
        {
            var result = employeeRepository.EmployeeCountByDept();
            return View(result);
        }
    }
}
