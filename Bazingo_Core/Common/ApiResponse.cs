using System.Collections.Generic;

namespace Bazingo_Core.Common
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public bool Succeeded { get; set; }
        public Dictionary<string, object> AdditionalData { get; set; }
        public List<string> Errors { get; set; }

        public ApiResponse()
        {
            Succeeded = true;
            Message = string.Empty;
            Errors = new List<string>();
            AdditionalData = new Dictionary<string, object>();
        }

        public ApiResponse(T data, string message = null)
        {
            Succeeded = true;
            Message = message;
            Data = data;
            Errors = new List<string>();
            AdditionalData = new Dictionary<string, object>();
        }

        public ApiResponse(string message)
        {
            Succeeded = false;
            Message = message;
            Errors = new List<string>();
            AdditionalData = new Dictionary<string, object>();
        }

        public ApiResponse(string message, List<string> errors)
        {
            Succeeded = false;
            Message = message;
            Errors = errors;
            AdditionalData = new Dictionary<string, object>();
        }
    }
}
