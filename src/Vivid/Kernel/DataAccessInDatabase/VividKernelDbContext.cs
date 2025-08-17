// GNU AFFERO GENERAL PUBLIC LICENSE
// Version 3, 19 November 2007
//
// Copyright (C) 2007 Free Software Foundation, Inc. <https://fsf.org/>
// Everyone is permitted to copy and distribute verbatim copies
// of this license document, but changing it is not allowed.

using Microsoft.EntityFrameworkCore;

namespace Vivid.Kernel.DataAccessInDatabase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VividKernelDbContext"/> class.   
    /// </summary>
    /// <param name="options">The options.</param>
    public class VividKernelDbContext(DbContextOptions<VividKernelDbContext> options) : DbContext(options)
    {
        /// <summary>
        /// Gets or sets the Customers table, which represents the <see cref="VividKernelCustomerModel"/> entity.
        /// </summary>
        public DbSet<VividKernelCustomerModel> Customers { get; set; }
    }
}