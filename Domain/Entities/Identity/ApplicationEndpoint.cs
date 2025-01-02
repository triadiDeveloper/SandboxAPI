using System.ComponentModel.DataAnnotations;
using Application.BaseEntity;

namespace Domain.Entities.Identity
{
    public class ApplicationEndpoint : BaseIdInt, IAudited, INote
    {
        [MaxLength(255)]
        public string Name { get; set; } = default!;
        public bool? IsAction { get; set; }
        [MaxLength(2000)]
        public string? Note { get; set; }
        [MaxLength(250)]
        public string? Platform { get; set; }
        [MaxLength(255)]
        public string? CreatedUser { get; set; }
        public DateTime? CreatedDate { get; set; }
        [MaxLength(255)]
        public string? ModifiedUser { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
