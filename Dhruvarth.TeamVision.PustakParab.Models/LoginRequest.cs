using System.ComponentModel.DataAnnotations;

namespace Dhruvarth.TeamVision.PustakParab.Models
{
    public class LoginRequest
    {
        [Required]
        public long MMobileNo { get; set; }
        [Required]
        public int MPIN { get; set; }
    }
}
