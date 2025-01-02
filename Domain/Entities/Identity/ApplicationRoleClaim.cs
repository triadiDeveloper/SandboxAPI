using Application.BaseEntity;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Identity
{
    public class ApplicationRoleClaim : IdentityRoleClaim<Guid>, IAudited
    {
        public int ApplicationControllerMethodId { get; set; }
        public virtual ApplicationControllerMethod? ApplicationControllerMethod { get; set; }
     
        public virtual ApplicationRole? ApplicationRole { get; set; }
        [MaxLength(255)]
        public string? CreatedUser { get; set; }
        public DateTime? CreatedDate { get; set; }
        [MaxLength(255)]
        public string? ModifiedUser { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
