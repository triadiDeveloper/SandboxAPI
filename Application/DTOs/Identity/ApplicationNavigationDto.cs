namespace Application.DTOs.Identity
{
    public class ApplicationNavigationDto
    {
        public string? ParentCode { get; set; }
        public string? Icon { get; set; }
        public string? Url { get; set; }
        public int? ApplicationControllerId { get; set; }
        public ApplicationControllerDto? ApplicationController { get; set; }
        public int Index { get; set; } // Urutan Navigation
    }
}
