namespace EmployeeService.DTOs
{
    /// <summary>
    /// Represents employee information returned by the API.
    /// </summary>
    public class EmployeeResponse
    {
        /// <summary>
        /// Unique employee ID.
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// Employee name.
        /// </summary>
        /// <example>Mayur</example>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Employee salary.
        /// </summary>
        /// <example>75000</example>
        public decimal Salary { get; set; }

        /// <summary>
        /// Employee Address.
        /// </summary>
        /// <example>Thane</example>
        public decimal Address { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}