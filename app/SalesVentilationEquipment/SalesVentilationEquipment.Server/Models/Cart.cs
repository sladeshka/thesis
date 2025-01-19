using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SalesVentilationEquipment.Server.Models
{
    public class Cart : BaseModel
    {
        [Column("id")]
        [Key]
        public override Guid Id { get; set; } = Guid.NewGuid();

        [Column("contractor_id")]
        [ForeignKey("Contractor")]
        public required Guid ContractorId { get; set; }

        [Required]
        public virtual Contractor Contractor { get; set; }

        [Column("total_sum")]
        [Required]
        public required double TotalSum { get; set; }

        [Column("discount")]
        public required double Discount { get; set; }
    }
}
