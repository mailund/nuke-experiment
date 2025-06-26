#pragma once

// Exhange
typedef void *NativeExchangeHandle;
typedef struct ExchangeVtable {
  int (*add_order)(NativeExchangeHandle, double quantity, double price);
  int (*update_order)(NativeExchangeHandle, int orderId, double quantity,
                      double price);
} ExchangeVtable;

// Agents
typedef void *AgentHandle;
typedef struct AgentVtable {
  void (*init_agent)(AgentHandle, int agentId, NativeExchangeHandle exchange,
                     struct ExchangeVtable *exchangeVtable);
  void (*event)(AgentHandle, int eventId);
  void (*free)(AgentHandle);
} AgentVtable;

typedef AgentHandle (*AgentFactory)(AgentVtable **vtable_out);
