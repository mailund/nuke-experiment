pub mod agent;
pub mod exchange;
pub mod native;

pub use agent::Agent;
pub use exchange::Exchange;

/// Macro that declares an agent factory function for a given agent type. You can only use it
/// once per dynlib due to the C interface that requires the name of agent factories to be agent_factory.
/// This is a limit of the current interface and not the Rust design.
#[macro_export]
macro_rules! declare_agent_factory {
    ($agent_type:ty) => {
        #[unsafe(no_mangle)]
        pub extern "C" fn agent_factory(
            vtable_out: *mut *const $crate::native::AgentVtable,
        ) -> *mut std::ffi::c_void {
            unsafe {
                *vtable_out = <$agent_type as $crate::agent::Agent>::vtable();
            };
            let agent = <$agent_type as $crate::agent::Agent>::new();
            Box::into_raw(Box::new(agent)) as *mut std::ffi::c_void
        }
    };
}
