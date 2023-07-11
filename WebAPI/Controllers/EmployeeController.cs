using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;
using System.Data;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Employee Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        /// <summary>
        /// Employee service.
        /// </summary>
        private IEmployeeService _employeeService;

        /// <summary>
        /// Auto Mapper
        /// </summary>
        private IMapper _mapper;

        /// <summary>
        /// Controller constructor, provide EmployeeService and Auto Mapper objects register in service collection. 
        /// </summary>
        /// <param name="employeeService">Employee service</param>
        /// <param name="mapper">Auto Mapper</param>
        public EmployeeController(IEmployeeService employeeService, IMapper mapper)
        {
            _employeeService = employeeService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get All Employees  
        /// </summary>
        /// <param name="pageNumber">page number</param>
        /// <param name="pageSize">Number of records want to fetch per page.</param>
        /// <returns>Employee records depnding upon the pagesize set in request parameter.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmployeeDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginationResponse<EmployeeDto>>> Get(int pageNumber = 1, int pageSize = 10)
        {
            var response = await _employeeService.GetAllAsync(pageNumber, pageSize);

            if (response.employees == null || !response.employees.Any())
                return NoContent();

            var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(response.employees);

            var pageResponse = new PaginationResponse<EmployeeDto>(pageSize, pageNumber, response.totalCount, employeeDtos);

            return Ok(pageResponse);
        }

        /// <summary>
        /// Search Employee.
        /// </summary>
        /// <param name="searchInput">search input string matches first name, last name of employee.</param>
        /// <param name="pageNumber">page number</param>
        /// <param name="pageSize">Number of employees want to fetch per page.</param>
        /// <returns>Employees mathces search input with first name, last name.</returns>
        /// <response code="400">If the item is null</response>       
        [HttpGet("Search")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmployeeDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginationResponse<EmployeeDto>>> Search(string searchInput, int pageNumber = 1, int pageSize = 10)
        {
            var response = await _employeeService.SearchEmployeeAsync(pageNumber, pageSize, searchInput);

            if (response.employees == null || !response.employees.Any())
                return NotFound();

            var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(response.employees);

            var pageResponse = new PaginationResponse<EmployeeDto>(pageSize, pageNumber, response.totalCount, employeeDtos);

            return Ok(pageResponse);
        }

        /// <summary>
        /// CreateAsync Employee
        /// </summary>
        /// <param name="employeeDto">Employee record to create.</param>
        /// <returns>Created Employee record.</returns>
        /// <response code="201">Returns the newly created employee</response>
        /// <response code="400">If the employee record is null or Request model is not valid.</response>       
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(EmployeeDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Post([FromBody] EmployeeDto employeeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);    

            var employee = _mapper.Map<Employee>(employeeDto);
           
            var isExists = await _employeeService.CheckEmployeeAlreadyExistsAsync(employee);

            if (isExists)
            {
                ModelState.AddModelError("", "Employee with FirstName, LastName and EmailAddress already exists.");
                return BadRequest(ModelState);
            }

            await _employeeService.CreateAsync(employee);

            var createdEmployeeDto = _mapper.Map<EmployeeDto>(employee);

            return CreatedAtAction(nameof(Post), new { id = employee.Id }, createdEmployeeDto);
        }

        /// <summary>
        /// DeleteAsync Employee
        /// </summary>
        /// <param name="id">Employee Id use to delete record.</param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);

            if(employee == null)
            {
                return NotFound();
            }

            await _employeeService.DeleteAsync(employee);

            return NoContent();
        }

        /// <summary>
        /// UpdateAsync Employee
        /// </summary>
        /// <param name="id">Employee Id</param>
        /// <param name="employeeDto">Employee Object To UpdateAsync</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Update(int id, [FromBody] EmployeeDto employeeDto)
        {
            if(id != employeeDto.Id)
            {
                return BadRequest();
            }
           
            var employee = _mapper.Map<Employee>(employeeDto);
           
            try
            {
                await _employeeService.UpdateAsync(employee);
            }
            catch (DBConcurrencyException) when (_employeeService.GetByIdAsync(employeeDto.Id).Result != null) 
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
