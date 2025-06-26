using System;
using System.Runtime.InteropServices;

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
        // Call the C function pointer for init_agent
        _vtable.init_agent(_agentHandle, agentId, exchange.CreateNativeHandle());
    }

    public void OnEvent(int eventId)
    {
        // Call the C function pointer for event
        _vtable.event_(_agentHandle, eventId);
    }
}

// C# representation of the C vtable struct
[StructLayout(LayoutKind.Sequential)]
public struct AgentVtable
{
    public InitAgentDelegate init_agent;
    public EventDelegate event_;
}

// Callback definitions for the vtable
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void InitAgentDelegate(IntPtr agentHandle, int agentId, IntPtr exchangeHandle);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void EventDelegate(IntPtr agentHandle, int eventId);
