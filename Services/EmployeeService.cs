using DAL;
using Microsoft.EntityFrameworkCore;
using Models;
using Services.Interfaces;

namespace Services
{
    public class EmployeeService : IEmployeeService
    {
        private EmployeeDBContext _employeeContext;

        public EmployeeService(EmployeeDBContext context)
        {
            _employeeContext = context;
        }

        public async Task<int> CreateAsync(Employee employee)
        {
            _employeeContext.Add(employee);
            return await _employeeContext.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Employee employee)
        {
            _employeeContext.Employees.Remove(employee);

            return await _employeeContext.SaveChangesAsync();
        }

        public async Task<(IEnumerable<Employee> employees, long totalCount)> GetAllAsync(int pageNumber, int pageSize)
        {
            var employees = await _employeeContext.Employees.Include(e => e.Addresses)
                                                            .Take(pageSize)
                                                            .Skip((pageNumber - 1) * pageSize)
                                                            .AsNoTracking().ToListAsync();


            var totalCount = await _employeeContext.Employees.AsNoTracking().CountAsync();

            return new(employees, totalCount);
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _employeeContext.Employees.FindAsync(id);
        }

        public async Task<bool> CheckEmployeeAlreadyExistsAsync(Employee employee)
        {
            return await _employeeContext.Employees.AnyAsync(e => e.FirstName == employee.FirstName
                                                               && e.LastName == employee.LastName
                                                               && e.EmailAddress == e.EmailAddress);
        }

        public async Task<int> UpdateAsync(Employee employee)
        {
            var employeeFromDB = await _employeeContext.Employees.Where(e => e.Id == employee.Id)
                                                             .Include(e => e.Addresses)
                                                             .SingleAsync();

            _employeeContext.Entry(employeeFromDB).CurrentValues.SetValues(employee);


            //Handle if any address is removed/deleted
            DeleteEmployeeAddressIfAny(employee, employeeFromDB);

            //UpdateAsync or Insert Addresses.
            UpdateInsertAddress(employee, employeeFromDB);

            return await _employeeContext.SaveChangesAsync();
        }

        public async Task<(IEnumerable<Employee> employees, long totalCount)> SearchEmployeeAsync(int pageNumber, int pageSize, string searchInput)
        {
            var employees = await _employeeContext.Employees.Where(a => a.FirstName.Contains(searchInput)
                                                                     || a.LastName.Contains(searchInput))
                                                            .Include(e => e.Addresses)
                                                            .Take(pageSize)
                                                            .Skip((pageNumber - 1) * pageSize)
                                                            .AsNoTracking().ToListAsync();

            var totalCount = await _employeeContext.Employees.AsNoTracking().CountAsync();

            return new(employees, totalCount);
        }

        private void UpdateInsertAddress(Employee employee, Employee employeeFromDB)
        {
            foreach (var address in employee.Addresses)
            {
                var addressToUpdate = employeeFromDB.Addresses.SingleOrDefault(e => e.Id.Equals(address.Id) && e.Id != default(int));

                if (addressToUpdate != null)
                {
                    address.Employee = addressToUpdate.Employee;
                    address.EmployeeId = addressToUpdate.EmployeeId;
                    _employeeContext.Entry(addressToUpdate).CurrentValues.SetValues(address);
                }
                else
                {
                    employeeFromDB.Addresses.Add(address);
                }
            }
        }

        private static void DeleteEmployeeAddressIfAny(Employee employee, Employee employeeFromDB)
        {
            var addressesToUpdate = employee.Addresses.Select(a => a.Id);

            var addressesToRemove = employeeFromDB.Addresses.Where(e => !addressesToUpdate.Contains(e.Id));

            foreach (var address in addressesToRemove)
            {
                employeeFromDB.Addresses.Remove(address);
            }
        }
    }
}