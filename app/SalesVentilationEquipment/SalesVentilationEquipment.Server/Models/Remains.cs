using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SalesVentilationEquipment.Server.Models
{
    public class Remains : BaseModel
    {
        [Column("id")]
        [Key]
        public override Guid Id { get; set; } = Guid.NewGuid();

        [Column("warehouse_id")]
        [ForeignKey("Warehouse")]
        public required Guid WarehouseId { get; set; }

        [Required]
        public virtual required Warehouse Warehouse { get; set; }

        [Column("product_id")]
        [ForeignKey("Product")]
        public required Guid ProductId { get; set; }

        [Required]
        public virtual required Product Product { get; set; }

        [Column("quantity")]
        [Required]
        public required double Quantity { get; set; }
    }
}
