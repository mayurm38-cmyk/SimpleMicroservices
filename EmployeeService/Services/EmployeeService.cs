using EmployeeService.Models;
using EmployeeService.Repositories;

namespace EmployeeService.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeService(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Employee?> UpdateAsync(
            int id,
            Employee employee)
        {
            return await _repository.UpdateAsync(id, employee);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<Employee> CreateAsync(Employee employee)
        {
            return await _repository.CreateAsync(employee);
        }


    }
}