using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Models;
using Moq;
using Services.Interfaces;
using System.Data;
using WebAPI.Controllers;
using WebAPI.Models;
using WebAPI.Profiles;
using Test.Helper;

namespace WebAPI.Tests.Controllers
{
    public class EmployeeControllerTests
    {   
        private readonly Mock<IEmployeeService> _mockEmployeeService;
        private readonly EmployeeController _employeeController;
        private readonly Mapper _mapper;

        /// <summary>
        /// Initialize Controller class.
        /// </summary>
        public EmployeeControllerTests() 
        {
            _mockEmployeeService = new Mock<IEmployeeService>();
            
            //Mapper setup
            var mapperConfiguration = new MapperConfiguration(config => { config.AddProfile(new MapperProfile()); });
            _mapper  = new Mapper(mapperConfiguration);

            _employeeController = new EmployeeController(_mockEmployeeService.Object, _mapper);
        }

        [Fact]
        public async Task GetEmployee_Returns_200OkResponse_Async()
        {
            //Arrange
            int pageNumber = 1, pageSize = 10;

            (IEnumerable<Employee> employees, long totalCount) employeeServiceResult = new(EmployeeMockData.GetEmployees(), 10);

            _mockEmployeeService.Setup(s => s.GetAllAsync(pageNumber, pageSize)).ReturnsAsync(value: employeeServiceResult);


            //Act
            var result = await _employeeController.Get(pageNumber, pageSize);

            //Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);

            Assert.Equal(okObjectResult.StatusCode, 200);
            _mockEmployeeService.Verify(s => s.GetAllAsync(pageNumber, pageSize), Times.Once);
        }

        [Fact]
        public async Task GetEmployee_Returns_204NoContent_Async()
        {
            //Arrange
            int pageNumber = 1, pageSize = 10;

            (IEnumerable<Employee> employees, long totalCount) employeeServiceResult = new(new List<Employee>(), 10);

            _mockEmployeeService.Setup(s => s.GetAllAsync(pageNumber, pageSize)).ReturnsAsync(value: employeeServiceResult);

            //Act
            var result = await _employeeController.Get(pageNumber, pageSize);

            //Assert
            var noContentResult = Assert.IsType<NoContentResult>(result.Result);

            Assert.Equal(204, noContentResult.StatusCode);
            _mockEmployeeService.Verify(s => s.GetAllAsync(pageNumber, pageSize), Times.Once);
        }

        [Fact]
        public async Task GetEmployee_Returns_Records_AsPerPage_Size_Async()
        {
            //Arrange
            int pageNumber = 1, pageSize = 10 ,totalCount = 20;

             (IEnumerable<Employee> employees, long totalCount) employeeServiceResult = new(EmployeeMockData.GetEmployees().Take(pageSize), totalCount);

            _mockEmployeeService.Setup(s => s.GetAllAsync(pageNumber, pageSize)).ReturnsAsync(value: employeeServiceResult);

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            //Act
            var result = await _employeeController.Get(pageNumber, pageSize);

            //Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var pagintaionResponse = Assert.IsType<PaginationResponse<EmployeeDto>>(okObjectResult.Value);

           
            Assert.Equal(okObjectResult.StatusCode, 200);
            Assert.Equal(pageSize, pagintaionResponse.Data.Count());
            Assert.Equal(totalPages, pagintaionResponse.TotalPages);

            _mockEmployeeService.Verify(s => s.GetAllAsync(pageNumber, pageSize), Times.Once);
        }

        [Fact]
        public async Task SearchEmployee_Returns_200OkResponse_Async()
        {
            //Arrange
            int pageNumber = 1, pageSize = 10;
            string searchInput = "Robby";

            (IEnumerable<Employee> employees, long totalCount) employeeServiceResult = new(EmployeeMockData.GetEmployees(), 10);

            _mockEmployeeService.Setup(s => s.SearchEmployeeAsync(pageNumber, pageSize, searchInput)).ReturnsAsync(value: employeeServiceResult);


            //Act
            var result = await _employeeController.Search(searchInput, pageNumber, pageSize);

            //Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);

            Assert.Equal(okObjectResult.StatusCode, 200);
            _mockEmployeeService.Verify(s => s.SearchEmployeeAsync(pageNumber, pageSize, searchInput), Times.Once);
        }

        [Fact]
        public async Task GetEmployee_Returns_404NotFound_Async()
        {
            //Arrange
            int pageNumber = 1, pageSize = 10;
            string searchInput = "test";

            (IEnumerable<Employee> employees, long totalCount) employeeServiceResult = new(new List<Employee>(), 10);

            _mockEmployeeService.Setup(s => s.SearchEmployeeAsync(pageNumber, pageSize, searchInput)).ReturnsAsync(value: employeeServiceResult);

            //Act
            var result = await _employeeController.Search(searchInput, pageNumber, pageSize);

            //Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);

            Assert.Equal(404, notFoundResult.StatusCode);
            _mockEmployeeService.Verify(s => s.SearchEmployeeAsync(pageNumber, pageSize, searchInput), Times.Once);
        }

        [Fact] 
        public async Task CreatEmployee_Returns_BadRequest_Async()
        {
            //Arrange
            EmployeeDto employeeDto = new EmployeeDto
            {
                FirstName = "Test",
            };
            var employee = _mapper.Map<Employee>(employeeDto);

            _employeeController.ModelState.AddModelError("LastName", "Required");

            _mockEmployeeService.Setup(m => m.CheckEmployeeAlreadyExistsAsync(employee)).ReturnsAsync(false);
            _mockEmployeeService.Setup(m => m.CreateAsync(employee)).ReturnsAsync(0);

            //Act
            var badResponse = await _employeeController.Post(employeeDto);

            //Assert
            Assert.IsType<BadRequestObjectResult>(badResponse);
            _mockEmployeeService.Verify(v => v.CheckEmployeeAlreadyExistsAsync(employee), Times.Never);
            _mockEmployeeService.Verify(v => v.CreateAsync(employee), Times.Never);
        }

        [Fact]
        public async Task CreatEmployee_Returns_BadRequest_If_RecordIsNotUnique_Async()
        {
            //Arrange
            var employee = EmployeeMockData.GetEmployees().First();
            var employeeDto = _mapper.Map<EmployeeDto>(employee);

            _mockEmployeeService.Setup(m => m.CheckEmployeeAlreadyExistsAsync(It.IsAny<Employee>())).ReturnsAsync(true);
            _mockEmployeeService.Setup(m => m.CreateAsync(It.IsAny<Employee>())).ReturnsAsync(0);

            //Act
            var badResponse = await _employeeController.Post(employeeDto);

            //Assert
             Assert.IsType<BadRequestObjectResult>(badResponse);
            _mockEmployeeService.Verify(v => v.CheckEmployeeAlreadyExistsAsync(It.IsAny<Employee>()), Times.Once);
            _mockEmployeeService.Verify(v => v.CreateAsync(It.IsAny<Employee>()), Times.Never);
        }

        [Fact]
        public async Task CreatEmployee_Returns_201CreatedResponse_Async()
        {
            //Arrange
            var employee = EmployeeMockData.GetEmployees().First();
            var employeeDto = _mapper.Map<EmployeeDto>(employee);

            _mockEmployeeService.Setup(m => m.CheckEmployeeAlreadyExistsAsync(It.IsAny<Employee>())).ReturnsAsync(false);
            _mockEmployeeService.Setup(m => m.CreateAsync(It.IsAny<Employee>())).ReturnsAsync(1);

            //Act
            var employeeResponse = await _employeeController.Post(employeeDto);

            //Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(employeeResponse);
            var acutalEmployeeDto = Assert.IsType<EmployeeDto>(createdAtActionResult.Value);

            Assert.Equal(employeeDto.FirstName, acutalEmployeeDto.FirstName);
            Assert.Equal(employeeDto.LastName, acutalEmployeeDto.LastName);
            Assert.Equal(employeeDto.EmailAddress, acutalEmployeeDto.EmailAddress);

            _mockEmployeeService.Verify(v => v.CheckEmployeeAlreadyExistsAsync(It.IsAny<Employee>()), Times.Once);
            _mockEmployeeService.Verify(v => v.CreateAsync(It.IsAny<Employee>()), Times.Once);
        }

        [Fact]
        public async Task DeleteEmployee_Returns_404NotFoundResponse_Async()
        {
            //Arrange
            int employeeIdToDelet = 2;
            Employee? employee = null;
            _mockEmployeeService.Setup(m => m.GetByIdAsync(employeeIdToDelet)).ReturnsAsync(employee);
            _mockEmployeeService.Setup(m => m.DeleteAsync(employee)).ReturnsAsync(0);

            //Act
            var employeeResponse = await _employeeController.Delete(employeeIdToDelet);

            //Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(employeeResponse);
           
            _mockEmployeeService.Verify(m => m.GetByIdAsync(employeeIdToDelet), Times.Once);
            _mockEmployeeService.Verify(m => m.DeleteAsync(employee), Times.Never);
        }

        [Fact]
        public async Task DeleteEmployee_Returns_204NoContentResponse_Async()
        {
            //Arrange
            int employeeIdToDelet = 2;
            Employee employee = EmployeeMockData.GetEmployees().Single(r => r.Id == employeeIdToDelet);

            _mockEmployeeService.Setup(m => m.GetByIdAsync(employeeIdToDelet)).ReturnsAsync(employee);
            _mockEmployeeService.Setup(m => m.DeleteAsync(employee)).ReturnsAsync(1);

            //Act
            var employeeResponse = await _employeeController.Delete(employeeIdToDelet);

            //Assert
            Assert.IsType<NoContentResult>(employeeResponse);
            _mockEmployeeService.Verify(m => m.GetByIdAsync(employeeIdToDelet), Times.Once);
            _mockEmployeeService.Verify(m => m.DeleteAsync(employee), Times.Once);
        }

        [Fact]
        public async Task UpdateEmployee_Returns_400BadRequest_Async()
        {
            //Arrange
            int employeeIdToUpdate = 2;
            var employee = new Employee { Id = 1 };

            var employeeDto = _mapper.Map<EmployeeDto>(employee);

            _mockEmployeeService.Setup(e => e.UpdateAsync(employee)).ReturnsAsync(0);

            //Act
            var result = await _employeeController.Update(employeeIdToUpdate, employeeDto);

            //Assert
            Assert.IsType<BadRequestResult>(result);
            _mockEmployeeService.Verify(e => e.UpdateAsync(employee), Times.Never);
        }

        [Fact]
        public async Task UpdateEmployee_Returns_204NoContent_Async()
        {
            //Arrange
            int employeeIdToUpdate = 2;
            var employee = EmployeeMockData.GetEmployees().Single(e => e.Id == employeeIdToUpdate);

            var employeeDto = _mapper.Map<EmployeeDto>(employee);

            _mockEmployeeService.Setup(e => e.UpdateAsync(It.IsAny<Employee>())).ReturnsAsync(1);

            //Act
            var result = await _employeeController.Update(employeeIdToUpdate, employeeDto);

            //Assert
            Assert.IsType<NoContentResult>(result);
            _mockEmployeeService.Verify(e => e.UpdateAsync(It.IsAny<Employee>()), Times.Once);
        }

        [Fact]
        public async Task UpdateEmployee_Returns_400BadRequest_IfEmployee_Record_Already_Deleted_Async()
        {
            //Arrange
            int employeeIdToUpdate = 2;
            var employee = EmployeeMockData.GetEmployees().Single(e => e.Id == employeeIdToUpdate);

            var employeeDto = _mapper.Map<EmployeeDto>(employee);
            _mockEmployeeService.Setup(e => e.UpdateAsync(It.IsAny<Employee>())).Throws<DBConcurrencyException>();
            _mockEmployeeService.Setup(e => e.GetByIdAsync(employeeIdToUpdate)).ReturnsAsync(employee);
           
            //Act
            var result = await _employeeController.Update(employeeIdToUpdate, employeeDto);

            //Assert
            Assert.IsType<NotFoundResult>(result);
            _mockEmployeeService.Verify(e => e.UpdateAsync(It.IsAny<Employee>()), Times.Once);
            _mockEmployeeService.Verify(e => e.GetByIdAsync(employeeIdToUpdate), Times.Once);
        }
    }
}
