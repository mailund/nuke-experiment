using System;

namespace Exchange;

public abstract class IAgent
{
    public int Id { get; }
    public string Name { get; }

    // Called when an event occurs on the exchange
    public abstract void OnEvent(int eventId);
}

