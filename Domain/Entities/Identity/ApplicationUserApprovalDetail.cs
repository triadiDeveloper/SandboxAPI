using Application.BaseEntity;
using Domain.Entities.Organization;

namespace Domain.Entities.Identity
{
    public class ApplicationUserApprovalDetail : BaseDomainDetail
    {
        public int ApplicationUserApprovalId { get; set; }
        public virtual ApplicationUserApproval? ApplicationUserApproval { get; set; }
        public int CompanyId { get; set; }
        public virtual Company? Company { get; set; }
    }

}
