namespace Dhruvarth.TeamVision.PustakParab.Models
{
    public class MemberModel
    {
        public int MID { get; set; }
        public string MName { get; set; }
        public string MAddress { get; set; }
        public long MMobileNo { get; set; }
        public int MPIN { get; set; }
        public long MWhatsAppNo { get; set; }
        public long MUIDAI { get; set; }
        public DateTime MJoiningDate { get; set; }
        public bool IsAdmin { get; set; }
    }
    public class LoginResponse : MemberModel
    {
        public string Token { get; set; } = string.Empty;
        public string? RefreshToken { get; set; } = string.Empty;
    }
}
