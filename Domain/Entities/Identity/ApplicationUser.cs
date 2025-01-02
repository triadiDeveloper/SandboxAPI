using Application.BaseEntity;
using Domain.Entities.HumanResource;
using Mapster;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser<Guid>, IAudited, IActivatable
    {
        [MaxLength(125)]
        public string? FirstName { get; set; }
        [MaxLength(125)]
        public string? LastName { get; set; }
        public byte[]? ProfilePicture { get; set; }
        public bool IsAzureUser { get; set; } = true;
        public bool? IsActive { get; set; } = true;
        [MaxLength(255)]
        public string? CreatedUser { get; set; }
        public DateTime? CreatedDate { get; set; }
        public byte[]? Version { get; set; }
        public string? SVersion { get; set; }
        [MaxLength(255)]
        public string? ModifiedUser { get; set; }
        public DateTime? ModifiedDate { get; set; }
        [MaxLength(255)]
        public string? PasswordOwn { get; set; }
        public int? EmployeeId { get; set; }
        [AdaptIgnore]
        public virtual Employee? Employee { get; set; }
        public ApplicationUser()
        {
            ApplicationUserCompanies = new HashSet<ApplicationUserCompany>();
            ApplicationUserRoles = new HashSet<ApplicationUserRole>();
            ApplicationUserTokens = new HashSet<ApplicationUserToken>();
            ApplicationUserLogins = new HashSet<ApplicationUserLogin>();
            ApplicationUserClaims = new HashSet<ApplicationUserClaim>();
            ApplicationUserInfos = new HashSet<ApplicationUserInfo>();
        }
        public virtual ICollection<ApplicationUserCompany> ApplicationUserCompanies { get; set; }
        public virtual ICollection<ApplicationUserClaim> ApplicationUserClaims { get; set; }
        public virtual ICollection<ApplicationUserLogin> ApplicationUserLogins { get; set; }
        public virtual ICollection<ApplicationUserToken> ApplicationUserTokens { get; set; }
        public virtual ICollection<ApplicationUserRole> ApplicationUserRoles { get; set; }
        public virtual ICollection<ApplicationUserInfo> ApplicationUserInfos { get; set; }
    }
}
