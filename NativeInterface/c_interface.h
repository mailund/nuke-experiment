#pragma once

// Exhange
typedef void *NativeExchangeHandle;

int exchange_add_order(NativeExchangeHandle handle, double quantity,
                       double price);
int exchange_update_order(NativeExchangeHandle handle, int orderId,
                          double quantity, double price);

typedef void *AgentHandle;
typedef struct AgentVtable {
  void (*init_agent)(AgentHandle, int agentId, NativeExchangeHandle exchange);
  void (*event)(AgentHandle, int eventId);
} AgentVtable;

typedef AgentHandle (*AgentFactory)(AgentVtable **vtable_out);