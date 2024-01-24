using Dapper;
using Dhruvarth.TeamVision.PustakParab.DbService;
using Dhruvarth.TeamVision.PustakParab.Models;

namespace Dhruvarth.TeamVision.PustakParab.Services
{
    public interface IMemberService
    {
        public Task<APIResponse> AddBookMember(MemberModel member);
    }

    public class MemberService : IMemberService
    {
        ISqlDbContext<object> sqlDbContext;
        ISqlDbConn sqlDbConn;
        public MemberService(ISqlDbConn _sqlDbConn)
        {
            sqlDbConn = _sqlDbConn;
            sqlDbContext = new SqlDbContext<object>(sqlDbConn);
        }
        public async Task<APIResponse> AddBookMember(MemberModel member)
        {
            try
            {
                DynamicParameters dynamicParameter = new DynamicParameters();
                dynamicParameter.Add("MName", member.MName);
                dynamicParameter.Add("MAddress", member.MAddress);
                dynamicParameter.Add("MMobileNo", member.MMobileNo);
                dynamicParameter.Add("MWhatsAppNo", member.MWhatsAppNo);
                dynamicParameter.Add("MUIDAI", member.MUIDAI);
                var IsAdded = sqlDbContext.Execute(@"SP_AddMember", dynamicParameter);
                return BaseResponse.SetResponse(true, IsAdded, "Member added successfully");
            }
            catch (Exception ex)
            {
                return BaseResponse.SetResponse(true, null, ex.Message);
            }
             
        }
    }
}
