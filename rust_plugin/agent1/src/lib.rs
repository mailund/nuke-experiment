use rust_plugin_lib::{declare_agent_factory, Agent, Exchange};
use std::os::raw::c_int;

pub struct AgentOne(Option<Box<Exchange<'static>>>);

impl Agent for AgentOne {
    fn new() -> Self {
        AgentOne(None)
    }
    fn init(&mut self, agent_id: c_int, _exchange: Exchange<'static>) {
        println!("AgentOne initialized with id {}", agent_id);
        self.0 = Some(Box::new(_exchange));
        self.0.as_ref().unwrap().add_order(1.0, 100.0);
    }
    fn event(&mut self, event_id: c_int) {
        println!("AgentOne received event {}", event_id);
        self.0.as_ref().unwrap().update_order(event_id, 1.0, 200.0);
    }
}

declare_agent_factory!(AgentOne);
