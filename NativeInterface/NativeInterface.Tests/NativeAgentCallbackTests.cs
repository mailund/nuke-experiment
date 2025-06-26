using System;
using System.Runtime.InteropServices;
using Xunit;
using NativeInterface;
using Shouldly;

public class NativeAgentCallbackTests
{
    private class TestNativeAgent
    {
        public int? CreatedOrderId { get; private set; }

        public void Init(IntPtr agentHandle, int agentId, IntPtr exchangeHandle, IntPtr exchangeVtablePtr)
        {
            // Marshal the vtable pointer to the correct struct
            var vtable = Marshal.PtrToStructure<NativeInterface.NativeExchangeHandle.ExchangeVTable>(exchangeVtablePtr);
            var addOrder = Marshal.GetDelegateForFunctionPointer<NativeInterface.NativeExchangeHandle.AddOrderDelegate>(vtable.AddOrder);

            // Actually add an order to the exchange
            CreatedOrderId = addOrder(exchangeHandle, 123.0, 456.0);
        }

        public void OnEvent(IntPtr agentHandle, int eventId) { }
    }

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

        IntPtr unmanagedVtable = Marshal.AllocHGlobal(Marshal.SizeOf<AgentVtable>());
        Marshal.StructureToPtr(vtable, unmanagedVtable, false);

        vtablePtr = unmanagedVtable;
        return (IntPtr)GCHandle.Alloc(native);
    }

    [Fact]
    public void NativeAgentFactory_AgentCanCallBackToExchange()
    {
        var factory = new NativeAgentFactory(FakeAgentFactory);
        var agent = factory.CreateAgent();
        var exchange = new Exchange.Exchange();

        // Act: This should trigger the agent's Init, which should (in real use) call back to the exchange.
        agent.Init(7, exchange);

        // Assert: The exchange should now have an order with the expected values.
        // (Assuming your agent really does create an order in the exchange in its Init.)
        var orders = exchange.GetAllOrders(); // You may need to implement this or use another way to enumerate orders.
        orders.ShouldNotBeEmpty();
    }
}
