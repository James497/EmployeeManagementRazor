using EmployeeManagementRazor.Models;

namespace EmployeeManagementRazor.Services.Interfaces
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAllEmployees();
    }
}
