using EmployeeService.Controllers;
using EmployeeService.Kafka;
using EmployeeService.Models;
using EmployeeService.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EmployeeService.Tests
{
    public class EmployeesControllerTests
    {
      
        private readonly Mock<IEmployeeService> _employeeServiceMock;
        private readonly Mock<IKafkaProducer> _kafkaProducerMock;
        private readonly EmployeesController _controller;

   
        public EmployeesControllerTests()
        {
            _employeeServiceMock = new Mock<IEmployeeService>();
            _kafkaProducerMock = new Mock<IKafkaProducer>();

            _controller = new EmployeesController(
                _employeeServiceMock.Object,
                _kafkaProducerMock.Object);
        }

        // TC-GET-001
        [Fact]
        public async Task GetById_WhenEmployeeExists_Returns200Ok()
        {
            // Arrange
            var employee = new Employee
            {
                Id = 1,
                Name = "Mayur",
                Salary = 75000,
                Address="Mumbai"
            };

            _employeeServiceMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(employee);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(200, okResult.StatusCode);

            var returnedEmployee =
                Assert.IsType<Employee>(okResult.Value);

            Assert.Equal(1, returnedEmployee.Id);
            Assert.Equal("Mayur", returnedEmployee.Name);
            Assert.Equal(75000, returnedEmployee.Salary);
            Assert.Equal("Mumbai", returnedEmployee.Address);
        }


        // TC-GET-002
        [Fact]
        public async Task GetById_WhenEmployeeNotFound_Returns404NotFound()
        {
            // Arrange
            _employeeServiceMock
                .Setup(x => x.GetByIdAsync(999))
                .ReturnsAsync((Employee?)null);

            // Act
            var result = await _controller.GetById(999);

            // Assert
            var notFoundResult =
                Assert.IsType<NotFoundObjectResult>(result);

            Assert.Equal(404, notFoundResult.StatusCode);
        }


        // TC-GET-003
        [Fact]
        public async Task GetById_WhenIdIsZero_Returns404NotFound()
        {
            // Arrange
            _employeeServiceMock
                .Setup(x => x.GetByIdAsync(0))
                .ReturnsAsync((Employee?)null);

            // Act
            var result = await _controller.GetById(0);

            // Assert
            var notFoundResult =
                Assert.IsType<NotFoundObjectResult>(result);

            Assert.Equal(404, notFoundResult.StatusCode);
        }


        // TC-GET-004
        [Fact]
        public async Task GetById_WhenIdIsNegative_Returns404NotFound()
        {
            // Arrange
            _employeeServiceMock
                .Setup(x => x.GetByIdAsync(-1))
                .ReturnsAsync((Employee?)null);

            // Act
            var result = await _controller.GetById(-1);

            // Assert
            var notFoundResult =
                Assert.IsType<NotFoundObjectResult>(result);

            Assert.Equal(404, notFoundResult.StatusCode);
        }


        // TC-GET-005
        [Fact]
        public async Task GetById_WhenServiceThrowsException_ThrowsException()
        {
            // Arrange
            _employeeServiceMock
                .Setup(x => x.GetByIdAsync(1))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _controller.GetById(1));

            Assert.Equal("Database error", exception.Message);
        }
    }
}