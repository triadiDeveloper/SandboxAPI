using System.ComponentModel.DataAnnotations;
using Application.BaseEntity;
using Domain.Entities.Organization;

namespace Domain.Entities.Identity
{
    public class ApplicationUserCompany : BaseIdInt, IAudited
    {
        public Guid ApplicationUserId { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }
        public int CompanyId { get; set; }
        public virtual Company? Company { get; set; }
        [MaxLength(255)]
        public string? CreatedUser { get; set; }
        public DateTime? CreatedDate { get; set; }
        [MaxLength(255)]
        public string? ModifiedUser { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
