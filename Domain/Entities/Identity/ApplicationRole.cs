using Application.BaseEntity;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Identity
{
    public class ApplicationRole : IdentityRole<Guid>, IAudited
    {
        [MaxLength(255)]
        public string? Description { get; set; }
        [MaxLength(255)]
        public string? CreatedUser { get; set; }
        public DateTime? CreatedDate { get; set; }
        [MaxLength(255)]
        public string? ModifiedUser { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public byte[]? Version { get; set; }
        public string? SVersion { get; set; }

        public ApplicationRole()
        {
            ApplicationUserRoles = new HashSet<ApplicationUserRole>();
            ApplicationRoleClaims = new HashSet<ApplicationRoleClaim>();
            ApplicationNavigationRoles = new HashSet<ApplicationNavigationRole>();
        }
        public ApplicationRole(string name, string? description = null)
        {
            Description = description;
            Name = name;

            ApplicationUserRoles = new HashSet<ApplicationUserRole>();
            ApplicationRoleClaims = new HashSet<ApplicationRoleClaim>();
            ApplicationNavigationRoles = new HashSet<ApplicationNavigationRole>();
        }

        public virtual ICollection<ApplicationUserRole> ApplicationUserRoles { get; set; }
        public virtual ICollection<ApplicationRoleClaim> ApplicationRoleClaims { get; set; }
        public virtual ICollection<ApplicationNavigationRole>? ApplicationNavigationRoles { get; set; }
    }
}
