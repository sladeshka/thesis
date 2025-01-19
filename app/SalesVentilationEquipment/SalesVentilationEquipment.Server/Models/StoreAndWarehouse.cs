using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SalesVentilationEquipment.Server.Models
{
    public class StoreAndWarehouse : BaseModel
    {
        [Column("id")]
        [Key]
        public override Guid Id { get; set; } = Guid.NewGuid();

        [Column("store_id")]
        [ForeignKey("Store")]
        public required Guid StoreId { get; set; }

        [Required]
        public virtual required Store Store { get; set; }

        [Column("warehouse_id")]
        [ForeignKey("Warehouse")]
        public required Guid WarehouseId { get; set; }

        [Required]
        public virtual required Warehouse Warehouse { get; set; }
    }
}
