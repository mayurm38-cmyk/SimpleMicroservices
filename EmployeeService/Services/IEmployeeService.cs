using EmployeeService.Models;

namespace EmployeeService.Services
{
    public interface IEmployeeService
    {
        Task<Employee?> GetByIdAsync(int id);

        Task<Employee?> UpdateAsync(
            int id,
            Employee employee);

        Task<bool> DeleteAsync(int id);

        Task<Employee> CreateAsync(Employee emloyee);

    }
}