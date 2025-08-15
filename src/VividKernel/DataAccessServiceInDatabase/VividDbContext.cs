using Microsoft.EntityFrameworkCore;
using VividKernelService.Model;

namespace VividKernelDataAccessServiceInDatabase
{
    public class VividDbContext : DbContext
    {
        public VividDbContext(DbContextOptions<VividDbContext> options)
            : base(options)
        {
        }

        public DbSet<CustomerModel> Customers { get; set; } = null!;
    }
}