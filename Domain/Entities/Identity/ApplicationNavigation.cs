using Application.BaseEntity;

namespace Domain.Entities.Identity
{
    public class ApplicationNavigation: BaseCodeName
    {
        public string? ParentCode { get; set; }
        public string? Icon { get; set; }
        public string? Url { get; set; }
        public int? ApplicationControllerId { get; set; }
        public ApplicationController? ApplicationController { get; set; }
        public int Index { get; set; } // Urutan Navigation
    }
}
