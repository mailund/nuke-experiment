using System;

namespace Exchange;

public abstract class IAgent
{
    public abstract void Init(int id, Exchange exchange);
    public abstract void OnEvent(int eventId);
}

