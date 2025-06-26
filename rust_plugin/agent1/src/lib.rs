use rust_plugin_lib::{declare_agent_factory, Agent, Exchange};
use std::os::raw::c_int;

pub struct AgentOne;

impl Agent for AgentOne {
    fn new() -> Self {
        AgentOne
    }
    fn init(&mut self, agent_id: c_int, _exchange: Exchange) {
        println!("AgentOne initialized with id {}", agent_id);
    }
    fn event(&mut self, event_id: c_int) {
        println!("AgentOne received event {}", event_id);
    }
}

declare_agent_factory!(AgentOne);
