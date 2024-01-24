namespace Dhruvarth.TeamVision.PustakParab.Models
{
    public class APIResponse
    {
        public APIResponse(bool result, object data, string message)
        {
            this.result = result;
            this.message = message;
            this.data = data;
        }
        public bool result { get; set; }
        public object data { get; set; }
        public string message { get; set; }
    }
}
