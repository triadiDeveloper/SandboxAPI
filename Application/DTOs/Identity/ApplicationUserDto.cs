using Domain.Entities.HumanResource;
using Domain.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Identity
{
    public class ApplicationUserDto
    {
        public ApplicationUserDto()
        {
            ApplicationUserRoles = new HashSet<ApplicationUserRole>();
            ApplicationUserCompanies = new HashSet<ApplicationUserCompany>();
        }
        [Required(ErrorMessage = "{0} is a mandatory field.")]
        [MaxLength(256, ErrorMessage = "The {0} can not have more than {1} characters")]
        public string UserName { get; set; } = String.Empty;

        public string? PasswordHash { get; set; }

        [MaxLength(256, ErrorMessage = "The {0} can not have more than {1} characters")]
        public string? Email { get; set; }

        //public int RoleId { get; set; }
        public virtual ICollection<ApplicationUserRole> ApplicationUserRoles { get; set; }
        public virtual ICollection<ApplicationUserCompany> ApplicationUserCompanies { get; set; }

        public bool IsAzureUser { get; set; }
        public bool IsActive { get; set; }

        [MaxLength(256, ErrorMessage = "The {0} can not have more than {1} characters")]
        public string FirstName { get; set; } = String.Empty;

        [MaxLength(256, ErrorMessage = "The {0} can not have more than {1} characters")]
        public string LastName { get; set; } = String.Empty;
        public string PhoneNumber { get; set; } = String.Empty;
        public string? PasswordOwn { get; set; }
        public int? EmployeeId { get; set; }
        public virtual Employee? Employee { get; set; }
    }
}
