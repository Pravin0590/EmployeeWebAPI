using Models;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class EmployeeDBContext : DbContext
    {
        public EmployeeDBContext()
        {
                    
        }
        public EmployeeDBContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Unique key contraint
            modelBuilder.Entity<Employee>().ToTable(nameof(Employee))                      
                        .HasIndex(e => new { e.FirstName, e.LastName, e.EmailAddress })
                        .IsUnique(true);

            modelBuilder.Entity<Address>().ToTable(nameof(Address));           
        }
    }
}