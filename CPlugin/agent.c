#include <stdio.h>
#include <stdlib.h>

// Exhange
typedef void *ExchangeHandle;
typedef struct ExchangeVtable {
  int (*add_order)(ExchangeHandle, double quantity, double price);
  int (*update_order)(ExchangeHandle, int orderId, double quantity,
                      double price);
} ExchangeVtable;

// Agents
typedef void *AgentHandle;
typedef struct AgentVtable {
  void (*init_agent)(AgentHandle, int agentId, ExchangeHandle exchange,
                     ExchangeVtable *exchangeVtable);
  void (*event)(AgentHandle, int eventId);
  void (*free)(AgentHandle);
} AgentVtable;

typedef AgentHandle (*AgentFactory)(AgentVtable **vtable_out);

typedef struct {
  int id;
  ExchangeHandle exchange;        // Assuming exchange is of type ExchangeHandle
  ExchangeVtable *exchangeVtable; // Pointer to the exchange vtable
} DummyAgent;

static void dummy_init_agent(AgentHandle agent, int agentId,
                             ExchangeHandle exchange,
                             ExchangeVtable *exchangeVtable) {
  DummyAgent *dummyAgent = (DummyAgent *)agent;
  dummyAgent->id = agentId;
  dummyAgent->exchange = exchange; // Store the exchange handle if needed
  dummyAgent->exchangeVtable = exchangeVtable; // Store the exchange vtable
  printf("DummyAgent initialized with id %d\n", agentId);
}

static void dummy_event(AgentHandle agent, int eventId) {
  DummyAgent *dummyAgent = (DummyAgent *)agent;
  printf("DummyAgent %d received event %d\n", dummyAgent->id, eventId);
  dummyAgent->exchangeVtable->add_order(dummyAgent->exchange, 10.0, 100.0);
}

static void dummy_free_agent(AgentHandle agent) {
  printf("DummyAgent with id %d freed\n", ((DummyAgent *)agent)->id);
  free(agent);
}

static void free_agent(AgentHandle agent) { dummy_free_agent(agent); }

AgentHandle agent_factory(AgentVtable **vtable_out) {
  static AgentVtable vtable = {dummy_init_agent, dummy_event, dummy_free_agent};
  *vtable_out = &vtable;
  DummyAgent *agent = (DummyAgent *)malloc(sizeof(DummyAgent));
  agent->id = 0;
  return (AgentHandle)agent;
}
