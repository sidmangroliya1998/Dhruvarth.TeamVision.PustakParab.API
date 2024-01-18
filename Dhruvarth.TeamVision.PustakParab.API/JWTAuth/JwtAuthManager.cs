using Dhruvarth.TeamVision.PustakParab.Services;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Dhruvarth.TeamVision.PustakParab.API.JWTAuth
{
    public interface IJwtAuthManager
    {
        JwtAuthResult GenerateTokens(string username, string emailid, Claim[] claims, DateTime now, string uniqueid);
        JwtAuthResult Refresh(string refreshToken, string emailid, string accessToken, DateTime now);
        void RemoveExpiredRefreshTokens(DateTime now);
        (ClaimsPrincipal, JwtSecurityToken) DecodeJwtToken(string token);
        void RemoveExpiredRefreshTokens(string userName);
    }
    public class JwtAuthManager : IJwtAuthManager
    {

        private readonly ConcurrentDictionary<string, RefreshToken> _usersRefreshTokens;  // can store in a database or a distributed cache
        private readonly JwtTokenConfig _jwtTokenConfig;
        private readonly IAuthService authService;
        private readonly byte[] _secret;

        public JwtAuthManager(JwtTokenConfig jwtTokenConfig, IAuthService authService)
        {
            _jwtTokenConfig = jwtTokenConfig;
            this.authService = authService;
            _usersRefreshTokens = new ConcurrentDictionary<string, RefreshToken>();
            _secret = Encoding.ASCII.GetBytes(jwtTokenConfig.Secret);
        }

        // optional: clean up expired refresh tokens
        public void RemoveExpiredRefreshTokens(DateTime now)
        {
            var expiredTokens = _usersRefreshTokens.Where(x => x.Value.ExpireAt < now).ToList();
            foreach (var expiredToken in expiredTokens)
            {
                if (_usersRefreshTokens.TryRemove(expiredToken.Key, out RefreshToken? refreshToken) && refreshToken != null)
                {
                    //authService.LogOut(refreshToken.Email);
                }
            }
            //RemoveExpiredSignals();
        }

        private void RemoveExpiredSignals()
        {
            //var expiredSignals=  SocketRunning.SocketSignalsList.Where(x => x.Time < DateTime.Now).ToList();
            //foreach (var expiredSignal in expiredSignals)
            //{
            //    SocketRunning.SocketSignalsList.Remove(expiredSignal);
            //}
        }


        public JwtAuthResult GenerateTokens(string username, string emailid, Claim[] claims, DateTime now, string uniqueid)
        {
            var shouldAddAudienceClaim = string.IsNullOrWhiteSpace(claims?.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Aud)?.Value);
            var jwtToken = new JwtSecurityToken(
                _jwtTokenConfig.Issuer,
                shouldAddAudienceClaim ? _jwtTokenConfig.Audience : string.Empty,
                claims,
                //expires: now.AddMinutes(10),
                expires: now.AddHours(_jwtTokenConfig.AccessTokenExpiration),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(_secret), SecurityAlgorithms.HmacSha256Signature));
            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            var refreshToken = new RefreshToken
            {
                UserName = username,
                Email = emailid,
                UserUniqueID = uniqueid,
                TokenString = GenerateRefreshTokenString(),
                ExpireAt = now.AddHours(_jwtTokenConfig.RefreshTokenExpiration)
            };
            _usersRefreshTokens.AddOrUpdate(refreshToken.TokenString, refreshToken, (s, t) => refreshToken);

            return new JwtAuthResult
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public JwtAuthResult Refresh(string refreshToken, string emailid, string accessToken, DateTime now)
        {
            var (principal, jwtToken) = DecodeJwtToken(accessToken);
            if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature))
            {
                throw new SecurityTokenException("Invalid token");
            }

            var userName = principal.Identity.Name;
            if (!_usersRefreshTokens.TryGetValue(refreshToken, out var existingRefreshToken))
            {
                throw new SecurityTokenException("Invalid token");
            }
            if (existingRefreshToken.UserName != userName || existingRefreshToken.ExpireAt < now)
            {
                throw new SecurityTokenException("Invalid token");
            }

            var result = GenerateTokens(userName, emailid, principal.Claims.ToArray(), now, _usersRefreshTokens[refreshToken].UserUniqueID);

            return result;
        }
        public bool AddUserInfoInRefreshToken(string userdetail, string uniqueID)
        {
            if (_usersRefreshTokens.TryGetValue(userdetail, out _))
            {
                _usersRefreshTokens[userdetail].UserUniqueID = uniqueID;
                return true;
            }
            return false;
        }

        public (ClaimsPrincipal, JwtSecurityToken) DecodeJwtToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new SecurityTokenException("Invalid token");
            }
            var principal = new JwtSecurityTokenHandler()
                .ValidateToken(token,
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = _jwtTokenConfig.Issuer,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(_secret),
                        ValidAudience = _jwtTokenConfig.Audience,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(1)
                    },
                    out var validatedToken);
            return (principal, validatedToken as JwtSecurityToken);
        }

        private static string GenerateRefreshTokenString()
        {
            var randomNumber = new byte[32];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }



        public void RemoveExpiredRefreshTokens(string userName)
        {
            var refreshTokens = _usersRefreshTokens.Where(x => x.Value.UserName == userName).ToList();
            foreach (var refreshToken in refreshTokens)
            {
                _usersRefreshTokens.TryRemove(refreshToken.Key, out _);
            }
        }
    }
}
