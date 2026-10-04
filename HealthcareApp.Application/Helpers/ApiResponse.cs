
using Healthcare.Domain.Entities.ApiResponse;

namespace HealthcareApp.Application.Helpers
{
    public class ApiResponse
    {
        protected APIResponse<T>  Ok<T> (T data, string message)  =>
            new () { Success = true, Message = message, Data = data };

        protected APIResponse<T> Fail<T>(string message) =>
            new() { Success = false, Message = message, Data = default };
    }
}
