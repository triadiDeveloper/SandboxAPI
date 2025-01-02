namespace Application.DTOs.Identity
{
    public class ApplicationNavigationRoleDto
    {
        public int Id { get; set; }
        public Guid ApplicationRoleRoleId { get; set; }
        public ApplicationRoleDto? ApplicationRole { get; set; }
        public int ApplicationNavigationId { get; set; }
        public ApplicationNavigationDto? ApplicationNavigation { get; set; }
    }
}
