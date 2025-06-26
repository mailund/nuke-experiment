using System;
using Exchange;
using System.Runtime.InteropServices;

namespace NativeInterface;

public class CNativeAgent : Exchange.IAgent
{
    private readonly IntPtr _agentHandle;
    private readonly AgentVtable _vtable;

    public CNativeAgent(IntPtr agentHandle, AgentVtable vtable)
    {
        _agentHandle = agentHandle;
        _vtable = vtable;
    }

    public void Init(int agentId, Exchange.Exchange exchange)
    {
        IntPtr exchangeVtablePtr = NativeExchangeHandle.CreateVTable(exchange);
        _vtable.init_agent(_agentHandle, agentId, exchange.CreateNativeHandle(), exchangeVtablePtr);
        // Optionally: store exchangeVtablePtr if you need to free it later
    }

    public void OnEvent(int eventId)
    {
        _vtable.event_(_agentHandle, eventId);
    }

    public void Dispose()
    {
        _vtable.free_agent(_agentHandle);
    }
}

// C# representation of the C vtable struct
[StructLayout(LayoutKind.Sequential)]
public struct AgentVtable
{
    public InitAgentDelegate init_agent;
    public EventDelegate event_;
    public FreeAgentDelegate free_agent;
}

// Callback definitions for the vtable
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void InitAgentDelegate(IntPtr agentHandle, int agentId, IntPtr exchangeHandle, IntPtr exchangeVtablePtr);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void EventDelegate(IntPtr agentHandle, int eventId);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void FreeAgentDelegate(IntPtr agentHandle);

