using EmployeeService.Models;
using EmployeeService.Repositories;
using Moq;
using Xunit;

namespace EmployeeService.Tests
{
    public class EmployeeServiceTests
    {
        private readonly Mock<IEmployeeRepository> _repositoryMock;

        public EmployeeServiceTests()
        {
            _repositoryMock =
                new Mock<IEmployeeRepository>();
        }

        [Fact]
        public async Task GetByIdAsync_WhenEmployeeExists_ReturnsEmployee()
        {
            // Arrange
            var employee = new Employee
            {
                Id = 1,
                Name = "Mayur",
                Salary = 75000
            };

            _repositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(employee);

            var service =
                new EmployeeService.Services.EmployeeService(
                    _repositoryMock.Object);

            // Act
            var result =
                await service.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Mayur", result.Name);
            Assert.Equal(75000, result.Salary);
        }
    }
}