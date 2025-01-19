using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SalesVentilationEquipment.Server.Models
{
    /// <summary>
    /// Utility class for implementing many between the сontractor and the store
    /// </summary>
    public class ContractorAndStore : BaseModel
    {
        [Column("id")]
        [Key]
        public override Guid Id { get; set; } = Guid.NewGuid();

        [Column("contractor_id")]
        [ForeignKey("Contractor")]
        public required Guid ContractorId { get; set; }

        [Required]
        public virtual required Contractor Contractor { get; set; }

        [Column("store_id")]
        [ForeignKey("Store")]
        public required Guid StoreId { get; set; }

        [Required]
        public virtual required Store Store { get; set; }
    }
}
