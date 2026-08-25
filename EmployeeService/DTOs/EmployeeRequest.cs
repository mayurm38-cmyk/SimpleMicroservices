using System.ComponentModel.DataAnnotations;

namespace EmployeeService.DTOs
{
    /// <summary>
    /// Represents the request used to create or update an employee.
    /// </summary>
    public class EmployeeRequest
    {
        /// <summary>
        /// Employee name.
        /// </summary>
        /// <example>Mayur</example>
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Employee salary.
        /// </summary>
        /// <example>75000</example>
        [Range(1, 100000000)]
        public decimal Salary { get; set; }

        /// <summary>
        /// Employee Address.
        /// </summary>
        /// <example>Pune</example>
        [StringLength(100)]
        public string Address { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}