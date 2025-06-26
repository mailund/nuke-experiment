use std::os::raw::{c_double, c_int, c_void};
use crate::native;

pub struct Exchange<'a> {
    pub(crate) exchange_ptr: *mut c_void,
    pub(crate) vtable: &'a native::ExchangeVtable,
}

impl<'a> Exchange<'a> {
    pub fn add_order(&self, price: c_double, volume: c_double) -> c_int {
        (self.vtable.add_order)(self.exchange_ptr, price, volume)
    }
    pub fn update_order(&self, order_id: c_int, price: c_double, volume: c_double) -> c_int {
        (self.vtable.update_order)(self.exchange_ptr, order_id, price, volume)
    }
}

