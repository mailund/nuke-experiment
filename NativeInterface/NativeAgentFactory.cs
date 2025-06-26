using System;
using System.Runtime.InteropServices;

namespace NativeInterface;

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

    public static NativeAgentFactory LoadFromLibrary(string libraryPath)
    {
        // Load the native library
        IntPtr libHandle = NativeLibrary.Load(libraryPath);

        // Get the address of the agent_factory symbol
        if (!NativeLibrary.TryGetExport(libHandle, "agent_factory", out IntPtr symbolPtr))
            throw new InvalidOperationException("Could not find 'agent_factory' symbol in library.");

        // Marshal the symbol as a delegate
        var factory = Marshal.GetDelegateForFunctionPointer<AgentFactory>(symbolPtr);

        return new NativeAgentFactory(factory);
    }
}