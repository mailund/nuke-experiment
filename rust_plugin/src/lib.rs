use std::os::raw::{c_double, c_int, c_void};

#[repr(C)]
pub struct ExchangeVtable {
    pub add_order: extern "C" fn(*mut c_void, c_double, c_double) -> c_int,
    pub update_order: extern "C" fn(*mut c_void, c_int, c_double, c_double) -> c_int,
}

#[repr(C)]
pub struct AgentVtable {
    pub init_agent: extern "C" fn(*mut c_void, c_int, *mut c_void, *const ExchangeVtable),
    pub event: extern "C" fn(*mut c_void, c_int),
    pub free: extern "C" fn(*mut c_void),
}

#[unsafe(no_mangle)]
pub extern "C" fn agent_factory(vtable_out: *mut *const AgentVtable) -> *mut c_void {
    extern "C" fn init_agent(
        _agent: *mut c_void,
        agent_id: c_int,
        _exchange: *mut c_void,
        _exchange_vtable: *const ExchangeVtable,
    ) {
        println!("Rust agent initialized with id {}", agent_id);
    }
    extern "C" fn event(_agent: *mut c_void, event_id: c_int) {
        println!("Rust agent received event {}", event_id);
    }
    extern "C" fn free(_agent: *mut c_void) {
        println!("Rust agent freed");
    }

    static AGENT_VTABLE: AgentVtable = AgentVtable {
        init_agent,
        event,
        free,
    };

    unsafe {
        *vtable_out = &AGENT_VTABLE;
    }
    // Return a dummy agent pointer
    std::ptr::null_mut()
}
