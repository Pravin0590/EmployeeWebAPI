using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<(IEnumerable<Employee> employees, long totalCount)> GetAllAsync(int pageNumber, int pageSize);

        Task<(IEnumerable<Employee> employees, long totalCount)> SearchEmployeeAsync(int pageNumber, int pageSize, string searchInput);

        Task<Employee?> GetByIdAsync(int id);

        Task<int> CreateAsync(Employee employee);
        Task<int> UpdateAsync(Employee employee);

        Task<int> DeleteAsync(Employee employee);

        Task<bool> CheckEmployeeAlreadyExistsAsync(Employee employee);
    }
}
