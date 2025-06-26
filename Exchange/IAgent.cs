using System;

namespace Exchange;

public interface IAgent : IDisposable
{
    public void Init(int id, Exchange exchange);
    public void OnEvent(int eventId);
}