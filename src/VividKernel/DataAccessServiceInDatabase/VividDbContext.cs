using Microsoft.EntityFrameworkCore;
using VividKernelService.Model;

namespace VividKernelDataAccessServiceInDatabase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VividDbContext"/> class.   
    /// </summary>
    /// <param name="options">The options.</param>
    public class VividDbContext(DbContextOptions<VividDbContext> options) : DbContext(options)
    {
        /// <summary>
        /// Gets or sets the Customers table, which represents the <see cref="CustomerModel"/> entity.
        /// </summary>
        public DbSet<CustomerModel> Customers { get; set; }
    }
}