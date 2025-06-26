using System;
using System.Runtime.InteropServices;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate IntPtr AgentFactory(out IntPtr vtablePtr);

public class NativeAgentFactory
{
    private readonly AgentFactory _factory;

    public NativeAgentFactory(AgentFactory factory)
    {
        _factory = factory;
    }

    public CNativeAgent CreateAgent()
    {
        IntPtr vtablePtr;
        IntPtr agentHandle = _factory(out vtablePtr);

        var vtable = Marshal.PtrToStructure<AgentVtable>(vtablePtr);
        return new CNativeAgent(agentHandle, vtable);
    }
}