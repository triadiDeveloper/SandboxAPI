using System.ComponentModel.DataAnnotations;
using Application.BaseEntity;

namespace Domain.Entities.Identity
{
    public class ApplicationControllerMethod : BaseIdInt, IAudited
    {
        [MaxLength(255)]
        public string Name { get; set; } = default!;
        public int ApplicationControllerId { get; set; }
        public virtual ApplicationController ApplicationController { get; set; } = default!;
        [MaxLength(255)]
        public string? CreatedUser { get; set; }
        public DateTime? CreatedDate { get; set; }
        [MaxLength(255)]
        public string? ModifiedUser { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
