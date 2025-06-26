using System;
using System.Collections.Generic;

namespace Exchange;

public class Exchange
{
    private readonly Dictionary<int, Order> _orders = new();
    private readonly Dictionary<int, Agent> _agents = new();
    private int _nextOrderId = 1;
    private int _nextAgentId = 1;

    public int AddOrder(double quantity, double price)
    {
        var order = new Order
        {
            Id = _nextOrderId++,
            Quantity = quantity,
            Price = price
        };
        _orders[order.Id] = order;
        return order.Id;
    }

    public bool UpdateOrder(int orderId, double newQuantity, double newPrice)
    {
        if (_orders.TryGetValue(orderId, out var order))
        {
            order.Quantity = newQuantity;
            order.Price = newPrice;
            return true;
        }
        return false;
    }

    public Order? GetOrder(int orderId)
    {
        _orders.TryGetValue(orderId, out var order);
        return order;
    }

    // Add an agent to the exchange and return its assigned ID
    public int ConnectAgent(Agent agent)
    {
        agent.GetType(); // Ensures agent is not null
        var id = _nextAgentId++;
        _agents[id] = agent;
        return id;
    }

    // Inform all agents of an event
    public void Event(int eventId)
    {
        foreach (var agent in _agents.Values)
        {
            agent.OnEvent(eventId);
        }
    }
}
