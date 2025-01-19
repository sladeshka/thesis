using Microsoft.EntityFrameworkCore;
using SalesVentilationEquipment.Server.Models;

namespace SalesVentilationEquipment.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cart> Cart { get; set; } = default!;
        public DbSet<Contractor> Contractor { get; set; } = default!;
        public DbSet<ContractorAndStore> ContractorAndStore { get; set; } = default!;
        public DbSet<Order> Order { get; set; } = default!;
        public DbSet<Product> Product { get; set; } = default!;
        public DbSet<ProductInCart> ProductInCart { get; set; } = default!;
        public DbSet<Remains> Remains { get; set; } = default!;
        public DbSet<Store> Store { get; set; } = default!;
        public DbSet<StoreAndWarehouse> StoreAndWarehouse { get; set; } = default!;
        public DbSet<Warehouse> Warehouse { get; set; } = default!;
    }
}
