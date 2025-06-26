using Shouldly;

namespace Exchange.Tests;

public class ExchangeTests
{
    [Fact]
    public void ConnectAgent_ShouldAssignUniqueId()
    {
        var exchange = new Exchange();
        var agent = new TestAgent();
        var id = exchange.ConnectAgent(agent);

        id.ShouldBe(1);
    }

    [Fact]
    public void GetOrder_ShouldReturnNull_WhenOrderDoesNotExist()
    {
        var exchange = new Exchange();
        var order = exchange.GetOrder(42);

        order.ShouldBeNull();
    }
}

public class TestAgent : IAgent
{
    public void Init(int id, Exchange exchange)
    {
        // Initialize agent with ID and exchange reference
    }

    public void OnEvent(int eventId)
    {
        // Handle event
    }
}