using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesVentilationEquipment.Server.Models
{
    [Table("product")]
    [JsonObject]
    public class Product : BaseModel
    {
        [Column("id")]
        [Key]
        public override Guid Id { get; set; } = Guid.NewGuid();

        [Column("name")]
        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }
        
        [Column("price")]
        [MaxLength(100)]
        public required double Price { get; set; }

        [Column("description")]
        [MaxLength(100)]
        public string? Description { get; set; }

        [Column("feature")]
        [MaxLength(100)]
        public string? Feature { get; set; }
    }
}
