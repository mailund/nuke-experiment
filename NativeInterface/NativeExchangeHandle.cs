using System;
using System.Runtime.InteropServices;
using Exchange;

public static class NativeExchangeHandle
{
    // Call this from C# to get a handle you can pass to C
    public static IntPtr CreateNativeHandle(this Exchange.Exchange exchange)
    {
        return (IntPtr)GCHandle.Alloc(exchange);
    }

    // Optionally, a method to get the Exchange back from a handle (for C# use)
    public static Exchange.Exchange? FromNativeHandle(IntPtr handle)
    {
        return GCHandle.FromIntPtr(handle).Target as Exchange.Exchange;
    }
}

