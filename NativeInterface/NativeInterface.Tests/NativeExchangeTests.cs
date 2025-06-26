using System;
using System.Runtime.InteropServices;
using Xunit;

public class NativeExchangeTests
{
    [Fact]
    public unsafe void AddOrder_ThroughFunctionPointer_Works()
    {
        var exchange = new Exchange.Exchange();
        IntPtr handle = exchange.CreateNativeHandle();

        // Get function pointer to the unmanaged method
        delegate* unmanaged[Cdecl]<IntPtr, double, double, int> addOrderPtr =
            (delegate* unmanaged[Cdecl]<IntPtr, double, double, int>)
                typeof(NativeExchange)
                    .GetMethod("AddOrder")!
                    .MethodHandle
                    .GetFunctionPointer();

        int orderId = addOrderPtr(handle, 42.0, 100.0);

        var order = exchange.GetOrder(orderId);
        Assert.NotNull(order);
        Assert.Equal(42.0, order.Quantity);
        Assert.Equal(100.0, order.Price);
    }


    [Fact]
    public unsafe void UpdateOrder_ThroughFunctionPointer_Works()
    {
        var exchange = new Exchange.Exchange();
        IntPtr handle = exchange.CreateNativeHandle();

        // Get function pointer to the unmanaged AddOrder method
        delegate* unmanaged[Cdecl]<IntPtr, double, double, int> addOrderPtr =
            (delegate* unmanaged[Cdecl]<IntPtr, double, double, int>)
                typeof(NativeExchange)
                    .GetMethod("AddOrder")!
                    .MethodHandle
                    .GetFunctionPointer();

        // Get function pointer to the unmanaged UpdateOrder method
        delegate* unmanaged[Cdecl]<IntPtr, int, double, double, int> updateOrderPtr =
            (delegate* unmanaged[Cdecl]<IntPtr, int, double, double, int>)
                typeof(NativeExchange)
                    .GetMethod("UpdateOrder")!
                    .MethodHandle
                    .GetFunctionPointer();

        int orderId = addOrderPtr(handle, 10.0, 50.0);
        int result = updateOrderPtr(handle, orderId, 20.0, 60.0);

        Assert.Equal(1, result);
        var order = exchange.GetOrder(orderId);
        Assert.NotNull(order);
        Assert.Equal(20.0, order.Quantity);
        Assert.Equal(60.0, order.Price);
    }
}
