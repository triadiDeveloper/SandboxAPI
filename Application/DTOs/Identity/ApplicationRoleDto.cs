using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Identity
{
    public class ApplicationRoleDto
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "{0} is a mandatory field.")]
        [MaxLength(256, ErrorMessage = "The {0} can not have more than {1} characters")]
        public string Name { get; set; } = String.Empty;
        public ICollection<ApplicationNavigationRoleDto>? NavigationRoles { get; set; }
        public ICollection<ApplicationRoleClaimDto>? Claims { get; set; }
    }
}
