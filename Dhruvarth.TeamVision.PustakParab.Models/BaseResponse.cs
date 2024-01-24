using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhruvarth.TeamVision.PustakParab.Models
{
    public class BaseResponse
    {
        public static APIResponse SetResponse(bool _result, object _data, string _message)
        {
            APIResponse aPIResponse = new APIResponse(_result, _data, _message);

            return aPIResponse;
        }
    }
}
