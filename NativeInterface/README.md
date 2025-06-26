The Native (i.e. C) interface describes how code communicates
through C bindings. This interface enables practically any languages to communicate with the C# exchange through native
agent interfaces.

Exchange:

 - ExchangeHandle (opaque handle to the exchange)
 - int add_order(ExchangeHandle, double quantity, double price)
 - int update_order(int orderId, double quantity, double price)

AgentFactory:
 - AgentFactory (function pointer to create an agent vtable)

AgentVtable:
 - AgentHandle (opaque handle to the agent)
 - void (*init_agent)(AgentHandle, int agentId, ExchangeHandle exchange)
 - void (*event)(AgentHandle, int eventId)

