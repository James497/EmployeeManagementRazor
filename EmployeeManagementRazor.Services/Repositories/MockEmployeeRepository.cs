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

        public Employee Update(Employee updatedEmployee)
        {
            var emp = _employeeList.FirstOrDefault(e => e.Id == updatedEmployee.Id);
            if(emp != null)
            {
                emp.Name = updatedEmployee.Name;
                emp.Email = updatedEmployee.Email;
                emp.Department = updatedEmployee.Department;
                emp.PhotoPath = updatedEmployee.PhotoPath;
            }
            return emp;
        }

        public Employee Add(Employee newEmployee)
        {
            newEmployee.Id = _employeeList.Max(x => x.Id) + 1;
            _employeeList.Add(newEmployee);
            return newEmployee;
        }
        public Employee Delete(int id)
        {
            var employeeToDelete = _employeeList.FirstOrDefault(e => e.Id == id);

            if (employeeToDelete != null)
            {
                _employeeList.Remove(employeeToDelete);
            }

            return employeeToDelete;
        }
        public IEnumerable<DeptHeadCount> EmployeeCountByDept(Dept? dept)
        {
            IEnumerable<Employee> employees = _employeeList;
            if(dept.HasValue)
            {
                employees = employees.Where(x => x.Department == dept.Value).ToList();
            }
            return employees.GroupBy(e => e.Department)
                            .Select(g => new DeptHeadCount()
                            {
                                Department = g.Key.Value,
                                Count = g.Count()
                            }).ToList();
        }
    }
}
