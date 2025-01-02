using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Application.BaseEntity;

namespace Domain.Entities.Identity
{
    public class ApplicationUserRole : IdentityUserRole<Guid>, IAudited
    {
        public virtual ApplicationUser? ApplicationUser { get; set; }
        [ForeignKey("Role")]
        public override Guid RoleId { get => base.RoleId; set => base.RoleId = value; }
        [ForeignKey("RoleId")]
        public virtual ApplicationRole? ApplicationRole { get; set; }
        [MaxLength(255)]
        public string? CreatedUser { get; set; }
        public DateTime? CreatedDate { get; set; }
        [MaxLength(255)]
        public string? ModifiedUser { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public byte[]? Version { get; set; }
        public string? SVersion { get; set; }
    }
}
