using Microsoft.JSInterop;

namespace TangyWeb_Server.Helpers
{
    public static class IJSRuntimeExtension
    {
        public static async ValueTask ToastrSuccess(this IJSRuntime jSRuntime, string message)
        {
            await jSRuntime.InvokeVoidAsync("showToastr", "success", message);
        }

        public static async ValueTask ToastrError(this IJSRuntime jSRuntime, string message)
        { 
            await jSRuntime.InvokeVoidAsync("showToastr", "error", message);
        }
    }
}
