use std::os::raw::{c_double, c_int, c_void};

#[repr(C)]
pub struct ExchangeVtable {
    pub add_order: extern "C" fn(*mut std::ffi::c_void, c_double, c_double) -> c_int,
    pub update_order: extern "C" fn(*mut std::ffi::c_void, c_int, c_double, c_double) -> c_int,
}

#[repr(C)]
pub struct AgentVtable {
    pub init_agent:
        extern "C" fn(*mut std::ffi::c_void, c_int, *mut c_void, *const std::ffi::c_void),
    pub event: extern "C" fn(*mut std::ffi::c_void, c_int),
    pub free: extern "C" fn(*mut std::ffi::c_void),
}
