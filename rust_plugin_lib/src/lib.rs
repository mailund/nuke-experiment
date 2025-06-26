#[macro_export]
macro_rules! declare_agent_factory {
    ($agent_type:ty) => {
        #[no_mangle]
        pub extern "C" fn agent_factory(
            vtable_out: *mut *const $crate::native::AgentVtable,
        ) -> *mut std::os::raw::c_void {
            println!(
                "[Rust] agent_factory called for {}",
                stringify!($agent_type)
            );
            //let agent = <$agent_type as $crate::agent::Agent>::new_agent_with_vtable(vtable_out);
            println!(
                "[Rust] agent_factory returning agent pointer: {:p}",
                &agent as *const _
            );
            //Box::into_raw(Box::new(agent)) as *mut std::os::raw::c_void
            0x123400000000 as *mut std::os::raw::c_void
        }
    };
}
// ...existing code...
