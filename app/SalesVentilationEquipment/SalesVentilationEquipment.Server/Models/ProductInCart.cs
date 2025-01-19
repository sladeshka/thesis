using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SalesVentilationEquipment.Server.Models
{
    public class ProductInCart : BaseModel
    {
        [Column("id")]
        [Key]
        public override Guid Id { get; set; } = Guid.NewGuid();

        [Column("cart_id")]
        [ForeignKey("Cart")]
        public required Guid CartId { get; set; }

        [Required]
        public virtual Cart Cart { get; set; }

        [Column("product_id")]
        [ForeignKey("Product")]
        public required Guid ProductId { get; set; }

        [Required]
        public virtual Product Product { get; set; }
        
        [Column("unit_price")]
        [Required]
        public required double UnitPrice { get; set; }
        
        [Column("quantity")]
        [Required]
        public required double Quantity { get; set; }
    }
}
