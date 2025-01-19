using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SalesVentilationEquipment.Server.Models
{
    public class Contractor : BaseModel
    {
        [Column("id")]
        [Key]
        public override Guid Id { get; set; } = Guid.NewGuid();

        [Column("name")]
        [Required]
        [MaxLength(150)]
        public required string Name { get; set; }

        [Column("contact_info")]
        [Required]
        [MaxLength(150)]
        public required string ContactInfo { get; set; }
    }
}
