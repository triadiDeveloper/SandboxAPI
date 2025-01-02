using Application.BaseEntity;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Identity
{
    public class ApplicationUserToken : IdentityUserToken<Guid>, IAudited
    {
        public virtual ApplicationUser? ApplicationUser { get; set; }
        [MaxLength(255)]
        public string? CreatedUser { get; set; }
        public DateTime? CreatedDate { get; set; }
        [MaxLength(255)]
        public string? ModifiedUser { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
