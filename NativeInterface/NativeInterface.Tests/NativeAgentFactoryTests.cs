using System;
using System.Runtime.InteropServices;
using Xunit;

public class NativeAgentFactoryTests
{
    // Fake native agent implementation for testing
    private class TestNativeAgent
    {
        public int? InitAgentId { get; private set; }
        public IntPtr? InitExchangeHandle { get; private set; }
        public int? LastEventId { get; private set; }

        public void Init(IntPtr agentHandle, int agentId, IntPtr exchangeHandle)
        {
            InitAgentId = agentId;
            InitExchangeHandle = exchangeHandle;
        }

        public void OnEvent(IntPtr agentHandle, int eventId)
        {
            LastEventId = eventId;
        }
    }

    // Fake C "factory" implemented in C#
    public static IntPtr FakeAgentFactory(out IntPtr vtablePtr)
    {
        var native = new TestNativeAgent();
        InitAgentDelegate initDel = native.Init;
        EventDelegate eventDel = native.OnEvent;

        var vtable = new AgentVtable
        {
            init_agent = initDel,
            event_ = eventDel
        };

        // Allocate the vtable in unmanaged memory
        IntPtr unmanagedVtable = Marshal.AllocHGlobal(Marshal.SizeOf<AgentVtable>());
        Marshal.StructureToPtr(vtable, unmanagedVtable, false);

        vtablePtr = unmanagedVtable;
        // Return a fake agent handle (could be IntPtr.Zero for this test)
        return (IntPtr)1234;
    }

    [Fact]
    public void NativeAgentFactory_CreatesAgentAndCallsVtable()
    {
        // Arrange
        var factory = new NativeAgentFactory(FakeAgentFactory);
        var agent = factory.CreateAgent();

        // Act
        var exchange = new Exchange.Exchange();
        agent.Init(7, exchange);
        agent.OnEvent(42);

        // Assert
        // You can extend this to check that the vtable was called as expected.
        // For this test, you might want to expose the TestNativeAgent instance or use a static for verification.
        // Here, we just check that the agent was created and methods can be called without error.
        Assert.NotNull(agent);
    }
}
