using Application.BaseEntity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Identity
{
    public class ApplicationUserInfo : BaseIdInt, IAudited
    {
        public Guid ApplicationUserId { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }
        [MaxLength(50)]
        public string IpAddress { get; set; } = default!;
        public DateTime LoginDate { get; set; } = default!;
        [MaxLength(50)]
        public string AppName { get; set; } = default!;
        [MaxLength(20)]
        public string AppVersion { get; set; } = default!;
        [MaxLength(50)]
        public string DeviceName { get; set; } = default!;
        [MaxLength(255)]
        public string? CreatedUser { get; set; }
        public DateTime? CreatedDate { get; set; }
        [MaxLength(255)]
        public string? ModifiedUser { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
