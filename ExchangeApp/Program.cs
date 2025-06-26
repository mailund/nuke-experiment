using System;
using System.Collections.Generic;
using Exchange;
using NativeInterface;

class Program
{
    static void Main(string[] args)
    {
        // List of dynamic libraries to load agent factories from
        var agentLibs = new List<string>(args);
        if (agentLibs.Count == 0)
        {
            Console.WriteLine("Usage: ExchangeApp <agentLib1> <agentLib2> ...");
            Environment.Exit(1);
        }

        var exchange = new Exchange.Exchange();

        foreach (var lib in agentLibs)
        {
            try
            {
                var factory = NativeAgentFactory.LoadFromLibrary(lib);
                var agent = factory.CreateAgent();
                exchange.ConnectAgent(agent);
                Console.WriteLine($"Loaded and connected agent from {lib}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load agent from {lib}: {ex.Message}");
            }
        }

        // Example: trigger an event for all agents
        exchange.Event(1);

        Console.WriteLine("All agents loaded and event triggered.");
    }
}
