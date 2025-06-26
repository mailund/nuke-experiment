using System;

namespace Exchange;

public abstract class Agent
{
    public int Id { get; }
    public string Name { get; }

    protected Agent(int id, string name)
    {
        Id = id;
        Name = name;
    }

    // Called when an event occurs on the exchange
    public abstract void OnEvent(int eventId);
}

