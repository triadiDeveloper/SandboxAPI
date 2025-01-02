using Application.BaseEntity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Identity
{
    public class ApplicationController : BaseIdInt, IAudited
    {
        public ApplicationController()
        {
            ApplicationControllerMethods = new HashSet<ApplicationControllerMethod>();
        }
        [MaxLength(255)]
        public string Name { get; set; } = default!;
        [MaxLength(255)]
        public string? CreatedUser { get; set; }
        public DateTime? CreatedDate { get; set; }
        [MaxLength(255)]
        public string? ModifiedUser { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public virtual ICollection<ApplicationControllerMethod> ApplicationControllerMethods { get; set; }
    }
}
