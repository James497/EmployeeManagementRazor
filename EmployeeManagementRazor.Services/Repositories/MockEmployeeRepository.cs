using EmployeeManagementRazor.Models;
using EmployeeManagementRazor.Services.Interfaces;

namespace EmployeeManagementRazor.Services.Repositories
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _employeeList;
        public MockEmployeeRepository()
        {
            _employeeList = new List<Employee>()
            {
                new Employee() { Id = 1, Name = "Mary", Department = Dept.HR,
                    Email = "mary@pragimtech.com", PhotoPath="mary.png" },
                new Employee() { Id = 2, Name = "John", Department = Dept.IT,
                    Email = "john@pragimtech.com", PhotoPath="john.png" },
                new Employee() { Id = 3, Name = "Sara", Department = Dept.IT,
                    Email = "sara@pragimtech.com", PhotoPath="sara.png" },
                new Employee() { Id = 4, Name = "David", Department = Dept.Payroll,
                    Email = "david@pragimtech.com" },
            };
        }

        public Employee GetEmployee(int id)
        {
            return _employeeList.FirstOrDefault(x => x.Id == id);    
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _employeeList;
        }
    }
}
