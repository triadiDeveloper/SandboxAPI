using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Identity
{
    public class ApplicationUserLogin : IdentityUserLogin<Guid>
    {
        public virtual ApplicationUser? ApplicationUser { get; set; }
    }
}
