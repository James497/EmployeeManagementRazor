using EmployeeManagementRazor.Models;

namespace EmployeeManagementRazor.Services.Interfaces
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAllEmployees();
        Employee GetEmployee(int id);
    }
}
