using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SalesVentilationEquipment.Server.Models
{
    public class Order : BaseModel
    {
        [Column("id")]
        [Key]
        public override Guid Id { get; set; } = Guid.NewGuid();

        [Column("contractor_id")]
        [ForeignKey("Contractor")]
        public required Guid ContractorId { get; set; }

        [Required]
        public virtual Contractor Contractor { get; set; }

        [Column("cart_id")]
        [ForeignKey("Cart")]
        public required Guid CartId { get; set; }

        [Required]
        public virtual Cart Cart { get; set; }

        [Column("order_status")]
        [Required]
        [MaxLength(50)]
        public required string OrderStatus { get; set; }
    }
}
