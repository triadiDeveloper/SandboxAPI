namespace Application.DTOs.Identity
{
    public class ApplicationUserApprovalDetailDto
    {
        public int Id { get; set; }
        public int ApplicationUserApprovalId { get; set; }
        public int PlantId { get; set; }
        public bool? IsActive { get; set; } = true;
        public bool? IsDeleted { get; set; } = false;
    }
}
