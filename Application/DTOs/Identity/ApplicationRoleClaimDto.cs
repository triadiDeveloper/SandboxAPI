namespace Application.DTOs.Identity
{
    public class ApplicationRoleClaimDto
    {
        public int Id { get; set; }
        public int ControllerMethodId { get; set; }
        public Guid RoleId { get; set; }
    }
}
