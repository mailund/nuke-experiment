use rust_plugin_lib::{declare_agent_factory, Agent, Exchange};
use std::os::raw::{c_int};

pub struct AgentTwo;

impl Agent for AgentTwo {
    fn new() -> Self { AgentTwo }
    fn init(&mut self, agent_id: c_int, _exchange: Exchange) {
        println!("AgentTwo initialized with id {}", agent_id);
    }
    fn event(&mut self, event_id: c_int) {
        println!("AgentTwo received event {}", event_id);
    }
}

declare_agent_factory!(AgentTwo);

