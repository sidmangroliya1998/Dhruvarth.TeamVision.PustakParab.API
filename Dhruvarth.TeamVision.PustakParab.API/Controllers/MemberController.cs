using Dhruvarth.TeamVision.PustakParab.Models;
using Dhruvarth.TeamVision.PustakParab.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dhruvarth.TeamVision.PustakParab.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        IMemberService _memberService;
        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpPost("AddMember")]
        public async Task<IActionResult> AddBookMember([FromBody] MemberModel member) 
        {
            var result = _memberService.AddBookMember(member);
            return Ok(result);
        }
    }
}
