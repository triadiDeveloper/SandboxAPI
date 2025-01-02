using Application.BaseEntity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Identity
{
    public class ApplicationNavigationRole : BaseDomainDeep
    {
        [ForeignKey("ApplicationNavigation")]
        public int ApplicationNavigationId { get; set; }
        public virtual ApplicationNavigation? ApplicationNavigation { get; set; }

        [ForeignKey("ApplicationRole")]
        public Guid ApplicationRoleId { get; set; }
        public virtual ApplicationRole? ApplicationRole { get; set; }
    }
}
