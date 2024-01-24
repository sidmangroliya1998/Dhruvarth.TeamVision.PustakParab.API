namespace Dhruvarth.TeamVision.PustakParab.Models
{
    public class UserModel
    {
        public int UserID { get; set; }
        public long MobileNo { get; set; }
        public int PIN { get; set; }
        public bool IsActive { get; set; }
        public string Token { get; set; } = string.Empty;
        public string? RefreshToken { get; set; } = string.Empty;
    }
}
