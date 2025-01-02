namespace Application.DTOs.Identity
{
    public class ApplicationUserInfoDto
    {
        public int UserId { get; set; }
        public string AppName { get; set; } = String.Empty;
        public string AppVersion { get; set; } = String.Empty;
        public string DeviceName { get; set; } = String.Empty;
    }
}
