#pragma once

// Exhange
typedef void *NativeExchangeHandle;

int exchange_add_order(NativeExchangeHandle handle, double quantity,
                       double price);
int exchange_update_order(NativeExchangeHandle handle, int orderId,
                          double quantity, double price);
