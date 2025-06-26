namespace Exchange;

public interface IAgent
{
    public void Init(int id, Exchange exchange);
    public void OnEvent(int eventId);
}