using DAL;
using Models;
using Moq;
using Moq.EntityFrameworkCore;
using Test.Helper;

namespace Services.Tests
{
    public class EmployeeServiceTests
    {     
        [Fact]
        public async Task CreateAsync_Successfully_Created_Employee_Record()
        {
            //Arrange
            var mockEmployeeContext = new Mock<EmployeeDBContext>();

            int numberOfRowsAffected = 1;

            mockEmployeeContext.Setup(e => e.SaveChangesAsync(default)).ReturnsAsync(numberOfRowsAffected).Verifiable();

            var employeeService = new EmployeeService(mockEmployeeContext.Object);

            var employee = new Employee() { FirstName = "Debra", LastName = "Burks", EmailAddress = "debra.burks@yahoo.com", Addresses = new List<Address> { new Address() { Id = 1, City = "Orchard Park", State = "NY", Street = "9273 Thorne Ave. ", ZipCode = "14127" } } };

            //Act
            var result = await employeeService.CreateAsync(employee);

            //Assert
            Assert.Equal(numberOfRowsAffected, result);
            mockEmployeeContext.Verify(e => e.SaveChangesAsync(default), Times.Once());    
        }

        [Fact]
        public async Task GetAllAsync_Return_Employees_AsPer_Page_Size()
        {
            //Arrange
            var mockEmployeeContext = new Mock<EmployeeDBContext>();

            int pageSize = 10, pageNumber = 1;

            var employees = EmployeeMockData.GetEmployees();

            int totalRecords = employees.Count();

            mockEmployeeContext.Setup(x => x.Employees).ReturnsDbSet(employees);

            var employeeService = new EmployeeService(mockEmployeeContext.Object);

            //Act
            var result = await employeeService.GetAllAsync(pageNumber, pageSize);

            //Assert
            Assert.Equal(pageSize, result.employees.Count());
            Assert.Equal(totalRecords, result.totalCount);
        }

        [Fact]
        public async Task GetByIdAsync_Return_Employee()
        {
            //Arrange
            var mockEmployeeContext = new Mock<EmployeeDBContext>();
            
            var employees = EmployeeMockData.GetEmployees();

            int empolyeeIdToFind = 2;

            var actualEmployee = employees.Single(r => r.Id == empolyeeIdToFind);

            mockEmployeeContext.Setup(x => x.Employees).ReturnsDbSet(employees);
            mockEmployeeContext.Setup(x => x.Employees.FindAsync(empolyeeIdToFind)).ReturnsAsync(actualEmployee);

            var employeeService = new EmployeeService(mockEmployeeContext.Object);

            //Act
            var result = await employeeService.GetByIdAsync(empolyeeIdToFind);

            //Assert
            Assert.Equal(actualEmployee, result);
            Assert.Equal(empolyeeIdToFind, result?.Id);
        }

        [Fact]
        public async Task GetByIdAsync_Not_Found_Any_Employee()
        {
            //Arrange
            var mockEmployeeContext = new Mock<EmployeeDBContext>();

            var employees = EmployeeMockData.GetEmployees();

            int empolyeeIdToFind = 100;

            Employee? actualEmployee = null;

            mockEmployeeContext.Setup(x => x.Employees).ReturnsDbSet(employees);
            mockEmployeeContext.Setup(x => x.Employees.FindAsync(empolyeeIdToFind)).ReturnsAsync(actualEmployee);

            var employeeService = new EmployeeService(mockEmployeeContext.Object);

            //Act
            var result = await employeeService.GetByIdAsync(empolyeeIdToFind);

            //Assert
            Assert.Null(result);
        }


        [Fact]
        public async Task CheckEmployeeAlreadyExistsAsync_Return_True()
        {
            //Arrange
            var mockEmployeeContext = new Mock<EmployeeDBContext>();

            var employees = EmployeeMockData.GetEmployees();

            var employeeAlreadyExists = employees.First(e => e.FirstName == "Debra"
                                                               && e.LastName == "Burks"
                                                               && e.EmailAddress == "debra.burks@yahoo.com");

            mockEmployeeContext.Setup(x => x.Employees).ReturnsDbSet(employees);

            var employeeService = new EmployeeService(mockEmployeeContext.Object);

            //Act
            var result = await employeeService.CheckEmployeeAlreadyExistsAsync(employeeAlreadyExists);

            //Assert
            Assert.True(result);
        }


        [Fact]
        public async Task CheckEmployeeAlreadyExistsAsync_Return_False()
        {
            //Arrange
            var mockEmployeeContext = new Mock<EmployeeDBContext>();

            var employees = EmployeeMockData.GetEmployees();

            var employeeNotExists = new Employee
            {
                FirstName = "Regenia",
                LastName = "Vaughan",
                EmailAddress = "regenia.vaughan@gmail.com"
            };

            mockEmployeeContext.Setup(x => x.Employees).ReturnsDbSet(employees);

            var employeeService = new EmployeeService(mockEmployeeContext.Object);

            //Act
            var result = await employeeService.CheckEmployeeAlreadyExistsAsync(employeeNotExists);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public async Task SearchEmployeeAsync_Return_Employee_SearchReult_AsPer_Page_Size()
        {
            //Arrange
            var mockEmployeeContext = new Mock<EmployeeDBContext>();

            int pageSize = 10, pageNumber = 1;

            string searchText = "Garry";

            var employees = EmployeeMockData.GetEmployees();

            var expectedEmployee = employees.Where(a => a.FirstName.Contains(searchText)
                                                     || a.LastName.Contains(searchText));

            int totalRecords = employees.Count();

            mockEmployeeContext.Setup(x => x.Employees).ReturnsDbSet(employees);

            var employeeService = new EmployeeService(mockEmployeeContext.Object);

            //Act
            var result = await employeeService.SearchEmployeeAsync(pageNumber, pageSize, searchText);

            //Assert
            Assert.Equal(expectedEmployee.Count(), result.employees.Count());
            Assert.Equal(totalRecords, result.totalCount);
        }
    }
}