using Application.BaseEntity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Identity
{
    public class ApplicationUserApproval : BaseDomainDetail
    {

        public ApplicationUserApproval()
        {
            ApplicationUserApprovalDetails = new HashSet<ApplicationUserApprovalDetail>();
        }
        public Guid UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }
        public Guid ApplicationRoleId { get; set; }
        public virtual ApplicationRole? ApplicationRole { get; set; }
        public DateTime DocumentDate { get; set; }
        [MaxLength(1000)]
        public string? Note { get; set; }
        public virtual ICollection<ApplicationUserApprovalDetail> ApplicationUserApprovalDetails { get; set; }
    }

}
