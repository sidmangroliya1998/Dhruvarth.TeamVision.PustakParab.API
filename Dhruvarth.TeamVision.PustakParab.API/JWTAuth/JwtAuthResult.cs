using System.Text.Json.Serialization;

namespace Dhruvarth.TeamVision.PustakParab.API.JWTAuth
{
    public class JwtAuthResult
    {
        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonPropertyName("refreshToken")]
        public RefreshToken RefreshToken { get; set; }
    }

    public class RefreshToken
    {
        [JsonPropertyName("username")]
        public string UserName { get; set; } = string.Empty;
        [JsonPropertyName("useruniqueid")]
        public string UserUniqueID { get; set; } = string.Empty;

        [JsonPropertyName("tokenString")]
        public string TokenString { get; set; } = string.Empty;

        [JsonPropertyName("emailid")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("expireAt")]
        public DateTime ExpireAt { get; set; }
    }
}
