
using Dapper;
using Dhruvarth.TeamVision.PustakParab.DbService;
using Dhruvarth.TeamVision.PustakParab.Models;

namespace Dhruvarth.TeamVision.PustakParab.Services
{
    public interface IAuthService 
    {
        public Task<LoginResponse?> LogIn(LoginRequest loginInfo);
    }
    public class AuthService : IAuthService
    {
        ISqlDbContext<object> sqlDbContext;
        ISqlDbContext<LoginResponse> sqlUserDbContext;
        ISqlDbConn sqlDbConn;

        public AuthService(ISqlDbConn _sqlDbConn)
        {
            sqlDbConn = _sqlDbConn;
            sqlDbContext = new SqlDbContext<object>(sqlDbConn);
            sqlUserDbContext = new SqlDbContext<LoginResponse>(sqlDbConn);
        }
        public async Task<LoginResponse?> LogIn(LoginRequest loginInfo)
        {
            if (loginInfo.MMobileNo != null && loginInfo.MMobileNo > 0 && loginInfo.MPIN != null && loginInfo.MPIN > 0)
            {
                DynamicParameters dynamicParameter = new DynamicParameters();
                dynamicParameter.Add("MMobileNo", loginInfo.MMobileNo);
                dynamicParameter.Add("MPIN", loginInfo.MPIN);
                LoginResponse user = await sqlUserDbContext.GetAsync(@"SP_Login", dynamicParameter);
                return user;
            }
            else
            {
                return null;
            }
        }
    }
}