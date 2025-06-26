using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Exchange;

namespace NativeInterface;

public static class NativeExchangeHandle
{
    // Keep delegates alive for the lifetime of the process
    private static readonly List<Delegate> _pinnedDelegates = new();

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

    // Define delegates matching the c_interface.h vtable for exchanges
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int AddOrderDelegate(IntPtr handle, double quantity, double price);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int UpdateOrderDelegate(IntPtr handle, int orderId, double quantity, double price);

    // Struct matching the vtable layout in c_interface.h
    [StructLayout(LayoutKind.Sequential)]
    public struct ExchangeVTable
    {
        public IntPtr AddOrder;
        public IntPtr UpdateOrder;
    }

    public static IntPtr CreateVTable(Exchange.Exchange exchange)
    {
        // Implement the delegates
        int AddOrderImpl(IntPtr handle, double quantity, double price)
        {
            var ex = FromNativeHandle(handle);
            return ex != null ? ex.AddOrder(quantity, price) : -1;
        }

        int UpdateOrderImpl(IntPtr handle, int orderId, double quantity, double price)
        {
            var ex = FromNativeHandle(handle);
            return ex != null ? (ex.UpdateOrder(orderId, quantity, price) ? 1 : 0) : -1;
        }

        // Create delegate instances and get function pointers
        AddOrderDelegate addOrderDel = AddOrderImpl;
        UpdateOrderDelegate updateOrderDel = UpdateOrderImpl;

        // Pin the delegates so they aren't GC'd
        _pinnedDelegates.Add(addOrderDel);
        _pinnedDelegates.Add(updateOrderDel);

        IntPtr addOrderPtr = Marshal.GetFunctionPointerForDelegate(addOrderDel);
        IntPtr updateOrderPtr = Marshal.GetFunctionPointerForDelegate(updateOrderDel);

        // Create the vtable struct
        ExchangeVTable vtable = new ExchangeVTable
        {
            AddOrder = addOrderPtr,
            UpdateOrder = updateOrderPtr
        };

        // Allocate unmanaged memory for the vtable and copy the struct
        IntPtr vtablePtr = Marshal.AllocHGlobal(Marshal.SizeOf<ExchangeVTable>());
        Marshal.StructureToPtr(vtable, vtablePtr, false);

        return vtablePtr;
    }
}

