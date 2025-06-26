using System;
using System.Runtime.InteropServices;
using Exchange;
using Xunit;

public class NativeAgentTests
{
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

    [Fact]
    public void CNativeAgent_CallsVtableFunctions()
    {
        var native = new TestNativeAgent();

        // Create delegates
        InitAgentDelegate initDel = native.Init;
        EventDelegate eventDel = native.OnEvent;

        // Create vtable
        var vtable = new AgentVtable
        {
            init_agent = initDel,
            event_ = eventDel
        };

        // Allocate a fake agent handle (could be IntPtr.Zero for this test)
        IntPtr agentHandle = (IntPtr)1234;

        // Wrap as CNativeAgent
        var agent = new CNativeAgent(agentHandle, vtable);

        // Create a dummy exchange and call Init
        var exchange = new Exchange.Exchange();
        agent.Init(42, exchange);

        Assert.Equal(42, native.InitAgentId);
        Assert.NotNull(native.InitExchangeHandle);

        // Call OnEvent
        agent.OnEvent(99);
        Assert.Equal(99, native.LastEventId);
    }
}
