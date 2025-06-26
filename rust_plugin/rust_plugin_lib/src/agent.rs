use crate::exchange::Exchange;
use crate::native;
use std::os::raw::c_int;

pub trait Agent: Sized {
    fn new() -> Self;
    fn vtable() -> &'static native::AgentVtable {
        // SAFETY: This static is unique per T and the function pointers are always the same for T.
        static VTABLE: once_cell::sync::OnceCell<native::AgentVtable> =
            once_cell::sync::OnceCell::new();
        VTABLE.get_or_init(|| agent_vtable_from_agent::<Self>())
    }

    fn init(&mut self, agent_id: c_int, exchange: Exchange<'static>);
    fn event(&mut self, event_id: c_int);
}

pub fn agent_vtable_from_agent<T: Agent>() -> native::AgentVtable {
    extern "C" fn init_agent<T: Agent>(
        agent: *mut std::ffi::c_void,
        agent_id: c_int,
        exchange_ptr: *mut std::ffi::c_void,
        exchange_vtable: *const std::ffi::c_void,
    ) {
        unsafe {
            // SAFETY: The agent pointer must be a Box<T> where T: Agent
            let agent = &mut *(agent as *mut T);
            let exchange = Exchange {
                exchange_ptr,
                vtable: &*(exchange_vtable as *const native::ExchangeVtable),
            };
            agent.init(agent_id, exchange);
        }
    }
    extern "C" fn event<T: Agent>(agent: *mut std::ffi::c_void, event_id: c_int) {
        unsafe {
            let agent = &mut *(agent as *mut T);
            agent.event(event_id);
        }
    }
    extern "C" fn free<T: Agent>(agent: *mut std::ffi::c_void) {
        unsafe {
            // SAFETY: The agent pointer must be a Box<T> where T: Agent
            let _boxed: Box<T> = Box::from_raw(agent as *mut T);
            // Dropping the box will call Drop, which calls free()
        }
    }
    native::AgentVtable {
        init_agent: init_agent::<T>,
        event: event::<T>,
        free: free::<T>,
    }
}
