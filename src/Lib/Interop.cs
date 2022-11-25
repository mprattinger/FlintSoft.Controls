using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Lib;

public static class Interop
{
    internal static ValueTask<object> RegisterOutsideClick(this IJSRuntime jsRuntime, ElementReference element, object caller, string methodName, bool clearOnFire = false)
    {
        return jsRuntime.InvokeAsync<object>("fstypeahead.registerOutsideClick", element, DotNetObjectReference.Create(caller), methodName, clearOnFire);
    }

    internal static ValueTask<object> CleanupInterop(this IJSRuntime jsRuntime, ElementReference element)
    {
        return jsRuntime.InvokeAsync<object>("fstypeahead.onOutsideClickClear", element);
    }
}
