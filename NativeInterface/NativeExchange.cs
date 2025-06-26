using System;
using System.Runtime.InteropServices;
using Exchange;


namespace NativeInterface;

public static class NativeExchange
{
    [UnmanagedCallersOnly(EntryPoint = "exchange_add_order")]
    public static int AddOrder(IntPtr handle, double quantity, double price)
    {
        var exchange = NativeExchangeHandle.FromNativeHandle(handle)!;
        return exchange.AddOrder(quantity, price);
    }

    [UnmanagedCallersOnly(EntryPoint = "exchange_update_order")]
    public static int UpdateOrder(IntPtr handle, int orderId, double quantity, double price)
    {
        var exchange = NativeExchangeHandle.FromNativeHandle(handle)!;
        return exchange.UpdateOrder(orderId, quantity, price) ? 1 : 0;
    }
}