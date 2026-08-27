using EmployeeService.DTOs;
using EmployeeService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EmployeeService.Kafka;
using System.Text.Json;

namespace EmployeeService.Controllers
{
    /// <summary>
    /// Provides APIs for managing employees.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IKafkaProducer _kafkaProducer;

        public EmployeesController(
    IEmployeeService employeeService,
    IKafkaProducer kafkaProducer)
        {
            _employeeService = employeeService;
            _kafkaProducer = kafkaProducer;
        }

        /// <summary>
        /// Retrieves an employee by ID.
        /// </summary>
        /// <remarks>
        /// Returns employee details for the specified employee ID.
        ///
        /// Example:
        ///
        /// GET /api/Employees/1
        /// </remarks>
        /// <param name="id">Unique employee ID.</param>
        /// <returns>Employee details.</returns>
        /// <response code="200">Employee found successfully.</response>
        /// <response code="401">
        /// JWT token is missing or invalid.
        /// </response>
        /// <response code="404">
        /// Employee was not found.
        /// </response>
        /// <response code="500">
        /// Internal server error.
        /// </response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EmployeeResponse),
            StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
        {
            var employee =
                await _employeeService.GetByIdAsync(id);

            if (employee == null)
            {
                return NotFound("Employee not found.");
            }

            return Ok(employee);
        }


        /// <summary>
        /// Updates an existing employee.
        /// </summary>
        /// <remarks>
        /// Updates the employee name ,salary and Address.
        ///
        /// Example request:
        ///
        /// PUT /api/Employees/1
        ///
        /// {
        ///     "name": "Mayur",
        ///     "salary": 75000,
        ///     "Address": "Mumbai"
        /// }
        /// </remarks>
        /// <param name="id">Unique employee ID.</param>
        /// <param name="employee">
        /// Employee information to update.
        /// </param>
        /// <response code="200">
        /// Employee updated successfully.
        /// </response>
        /// <response code="400">
        /// Invalid request data.
        /// </response>
        /// <response code="401">
        /// JWT token is missing or invalid.
        /// </response>
        /// <response code="404">
        /// Employee was not found.
        /// </response>
        /// <response code="500">
        /// Internal server error.
        /// </response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(EmployeeResponse),
            StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(
            int id,
            EmployeeRequest employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employeeModel = new Models.Employee
            {
                Name = employee.Name,
                Salary = employee.Salary,
                Address = employee.Address
            };

            var updatedEmployee =
                await _employeeService
                    .UpdateAsync(id, employeeModel);

            if (updatedEmployee == null)
            {
                return NotFound("Employee not found");
            }

            return Ok(updatedEmployee);
        }


        /// <summary>
        /// Deletes an employee by ID.
        /// </summary>
        /// <remarks>
        /// Permanently deletes the employee record.
        ///
        /// Example:
        ///
        /// DELETE /api/Employees/1
        /// </remarks>
        /// <param name="id">Unique employee ID.</param>
        /// <response code="204">
        /// Employee deleted successfully.
        /// </response>
        /// <response code="401">
        /// JWT token is missing or invalid.
        /// </response>
        /// <response code="404">
        /// Employee was not found.
        /// </response>
        /// <response code="500">
        /// Internal server error.
        /// </response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted =
                await _employeeService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound("Employee not found");
            }

            return NoContent();
        }

        /// <summary>
        /// Creates a new employee.
        /// </summary>
        /// <remarks>
        /// Creates an employee record in the database.
        ///
        /// Example request:
        ///
        /// POST /api/Employees
        ///
        /// {
        ///     "name": "Mayur",
        ///     "salary": 75000,
        ///     "Address": "Mumbai"
        /// }
        /// </remarks>
        /// <param name="request">
        /// Employee information.
        /// </param>
        /// <response code="201">
        /// Employee created successfully.
        /// </response>
        /// <response code="400">
        /// Invalid employee data.
        /// </response>
        /// <response code="401">
        /// JWT token is missing or invalid.
        /// </response>
        /// <response code="500">
        /// Internal server error.
        /// </response>
        [HttpPost]
        [ProducesResponseType(
    typeof(EmployeeResponse),
    StatusCodes.Status201Created)]
        [ProducesResponseType(
    StatusCodes.Status400BadRequest)]
        [ProducesResponseType(
    StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(
    StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create(
    EmployeeRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = new Models.Employee
            {
                Name = request.Name,
                Salary = request.Salary,
                Address = request.Address,
                Email = request.Email
            };

            var createdEmployee =
                await _employeeService.CreateAsync(employee);

            // Kafka Event
            var message = JsonSerializer.Serialize(createdEmployee);

            await _kafkaProducer.PublishAsync(
                "employee-events",
                message);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdEmployee.Id },
                createdEmployee);
        }

    }
}