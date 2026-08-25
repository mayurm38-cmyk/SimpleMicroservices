using EmployeeService.Models;

namespace EmployeeService.Repositories
{
    public interface IEmployeeRepository
    {
        Task<Employee?> GetByIdAsync(int id);

        Task<Employee?> UpdateAsync(
            int id,
            Employee employee);

        Task<bool> DeleteAsync(int id);

        Task<Employee> CreateAsync(Employee employee);
    }
}