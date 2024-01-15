using System.Data.Entity;
using System.Net.Sockets;

namespace ITSupportTicketSystem.Models // Replace with the actual namespace
{
    public class SupportContext : DbContext
    {
        public SupportContext() : base("name=DefaultConnection") // Use the actual connection string name
        {
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        // Additional configuration for the model
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Example: modelBuilder.Entity<Department>().ToTable("Departments");
        }
    }
}
