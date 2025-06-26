use std::os::raw::{c_int, c_void, c_double};
use std::io::Write;

#[repr(C)]
pub struct ExchangeVtable {
    pub add_order: extern "C" fn(*mut c_void, c_double, c_double) -> c_int,
    pub update_order: extern "C" fn(*mut c_void, c_int, c_double, c_double) -> c_int,
}

pub struct Exchange<'a> {
    pub exchange_ptr: *mut c_void,
    pub vtable: &'a ExchangeVtable,
}

impl<'a> Exchange<'a> {
    pub fn add_order(&self, price: c_double, volume: c_double) -> c_int {
        (self.vtable.add_order)(self.exchange_ptr, price, volume)
    }
    pub fn update_order(&self, order_id: c_int, price: c_double, volume: c_double) -> c_int {
        (self.vtable.update_order)(self.exchange_ptr, order_id, price, volume)
    }
}

#[repr(C)]
pub struct AgentVtable {
    pub init_agent: extern "C" fn(*mut c_void, c_int, *mut c_void, *const ExchangeVtable),
    pub event: extern "C" fn(*mut c_void, c_int),
    pub free: extern "C" fn(*mut c_void),
}

pub trait Agent: Sized {
    fn new() -> Self;
    fn init(&mut self, agent_id: c_int, exchange: Exchange);
    fn event(&mut self, event_id: c_int);
    fn free(&mut self) {}
}

pub struct AgentOne;

impl Agent for AgentOne {
    fn new() -> Self { AgentOne }
    fn init(&mut self, agent_id: c_int, _exchange: Exchange) {
        println!("AgentOne initialized with id {}", agent_id);
    }
    fn event(&mut self, event_id: c_int) {
        println!("AgentOne received event {}", event_id);
    }
}

pub fn agent_vtable_from_agent<T: Agent>() -> AgentVtable {
    extern "C" fn init_agent<T: Agent>(
        agent: *mut c_void,
        agent_id: c_int,
        exchange_ptr: *mut c_void,
        exchange_vtable: *const ExchangeVtable,
    ) {
        unsafe {
            let agent = &mut *(agent as *mut T);
            let exchange = Exchange {
                exchange_ptr,
                vtable: &*exchange_vtable,
            };
            agent.init(agent_id, exchange);
        }
    }
    extern "C" fn event<T: Agent>(agent: *mut c_void, event_id: c_int) {
        unsafe {
            let agent = &mut *(agent as *mut T);
            agent.event(event_id);
        }
    }
    extern "C" fn free<T: Agent>(agent: *mut c_void) {
        unsafe {
            let _boxed: Box<T> = Box::from_raw(agent as *mut T);
        }
    }
    AgentVtable {
        init_agent: init_agent::<T>,
        event: event::<T>,
        free: free::<T>,
    }
}

#[no_mangle]
pub extern "C" fn agent_factory(vtable_out: *mut *const AgentVtable) -> *mut c_void {
    eprintln!("[Rust] agent_factory called");
    std::io::stderr().flush().unwrap();
    unsafe {
        *vtable_out = &agent_vtable_from_agent::<AgentOne>();
    }
    let agent = Box::new(AgentOne::new());
    Box::into_raw(agent) as *mut c_void
}

