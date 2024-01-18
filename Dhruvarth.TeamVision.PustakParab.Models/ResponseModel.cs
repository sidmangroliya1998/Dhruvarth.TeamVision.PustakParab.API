using System.Text.Json;

namespace Dhruvarth.TeamVision.PustakParab.Models
{
    public class ResponseModel
    {
        public int Key { get; set; }
        public string Message { get; set; } = String.Empty;
        public bool IsSuccess { get; set; }
        public object? Data { get; set; }
        public ResponseModel(int key, string message, object data) :
            this(key, message, true, data) { }

        public ResponseModel(int key, string message, bool IsSuccess, object data)
        {
            Message = message;
            this.IsSuccess = IsSuccess;
            Key = key;
            if (data != null)
            {
                Data = JsonSerializer.Serialize(data).Replace("\\", "");
            }
        }
        public ResponseModel() { }
    }
}
