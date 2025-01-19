using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SalesVentilationEquipment.Server.Models
{
    public class Store : BaseModel
    {
        [Column("id")]
        [Key]
        public override Guid Id { get; set; } = Guid.NewGuid();

        [Column("name")]
        [Required]
        [MaxLength(150)]
        public required string Name { get; set; }

        [Column("address")]
        [Required]
        [MaxLength(150)]
        public required string Address { get; set; }
    }
}
