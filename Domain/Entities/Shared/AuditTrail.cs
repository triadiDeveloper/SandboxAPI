using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Shared
{
    public class AuditTrail
    {
        /// <summary>
        /// Masih ada error ini nih untuk AuditEntry saat buat migration
        /// The property 'AuditEntry.Changes' is a collection or enumeration type with a value converter but with no value comparer. Set a value comparer to ensure the collection/enumeration elements are compared correctly.
        /// Microsoft.EntityFrameworkCore.Infrastructure[10403]
        /// </summary>
        /// 
        public long Id { get; set; }

        [MaxLength(255)]
        public string? EntityName { get; set; }

        [MaxLength(255)]
        public string? TableName { get; set; }

        [MaxLength(255)]
        public string? ActionType { get; set; }

        [MaxLength(255)]
        public string? Username { get; set; }

        [MaxLength(255)]
        public string? IpAddress { get; set; }

        public DateTime TimeStamp { get; set; }

        [MaxLength(255)]
        public string? EntityId { get; set; }

        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public string? AffectedColumns { get; set; }
    }
}
