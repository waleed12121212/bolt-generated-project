using System.Collections.Generic;

namespace Bazingo_Core.Models.Common
{
    public class ApiResponse<T>
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public T Data { get; set; }
        public Dictionary<string, object> AdditionalData { get; set; }

        public ApiResponse()
        {
            Errors = new List<string>();
            AdditionalData = new Dictionary<string, object>();
        }

        public static ApiResponse<T> CreateSuccess(T data, string message = null)
        {
            return new ApiResponse<T>
            {
                Succeeded = true,
                Message = message,
                Data = data
            };
        }

        public static ApiResponse<T> CreateError(string message)
        {
            return new ApiResponse<T>
            {
                Succeeded = false,
                Message = message
            };
        }

        public static ApiResponse<T> CreateError(List<string> errors)
        {
            return new ApiResponse<T>
            {
                Succeeded = false,
                Errors = errors
            };
        }
    }
}
