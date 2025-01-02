namespace Application.DTOs.Identity
{
    public class ApplicationUserApprovalDto
    {
        public ApplicationUserApprovalDto()
        {
            ApplicationUserApprovalDetails = new HashSet<ApplicationUserApprovalDetailDto>();
        }
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public DateTime DocumentDate { get; set; }
        public string? Note { get; set; }
        public bool? IsActive { get; set; } = true;
        public bool? IsDeleted { get; set; } = false;

        public ICollection<ApplicationUserApprovalDetailDto> ApplicationUserApprovalDetails { get; set; }
    }
}
