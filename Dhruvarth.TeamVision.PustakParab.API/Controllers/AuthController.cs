using Dhruvarth.TeamVision.PustakParab.API.JWTAuth;
using Dhruvarth.TeamVision.PustakParab.Models;
using Dhruvarth.TeamVision.PustakParab.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dhruvarth.TeamVision.PustakParab.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        //private readonly IMapper mapper;
        private readonly IJwtAuthManager jwtAuthManager;

        public AuthController(IAuthService _authService, IJwtAuthManager jwtAuthManager)
        {
            authService = _authService;
            this.jwtAuthManager = jwtAuthManager;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ResponseModel> LogIn([FromBody] LoginRequest _loginRequest)
        {
            try
            {
                var user = await authService.LogIn(_loginRequest);
                if (user != null)
                {
                    var claims = new[] { new Claim(ClaimTypes.MobilePhone, Convert.ToString(_loginRequest.MPIN)) };
                    string uniqueid = DateTime.Now.ToString("yyyyMMddHHmmss");

                    var jwtResult = jwtAuthManager.GenerateTokens(Convert.ToString(user.MobileNo), Convert.ToString(user.PIN), claims, DateTime.Now, uniqueid);
                    user.Token = jwtResult.AccessToken;
                    user.RefreshToken = Convert.ToString(jwtResult.RefreshToken);
                    return new ResponseModel(200, "User available", user);
                }
                throw new Exception("Mobile No or PIN is not correct.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
