namespace EmployeeService.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal Salary { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }
    }
}

