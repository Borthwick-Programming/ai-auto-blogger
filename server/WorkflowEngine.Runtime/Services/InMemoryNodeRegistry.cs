using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowEngine.Domain.Models;
using WorkflowEngine.Runtime.Interfaces;

namespace WorkflowEngine.Runtime.Services
{
    /// <summary>
    /// In-memory implementation of INodeRegistry.
    /// This simple version stores nodes in a local list and is suitable for development or testing.
    /// </summary>
    public class InMemoryNodeRegistry : INodeRegistry
    {
        // Internal list that holds node definitions in memory
        private readonly List<NodeDefinition> _definitions = new();

        /// <summary>
        /// Returns all node definitions currently in the in-memory list.
        /// </summary>
        public IEnumerable<NodeDefinition> GetAll() => _definitions;

        /// <summary>
        /// Adds a new node definition to the in-memory registry.
        /// </summary>
        /// <param name="definition">The node definition to register</param>
        public void Register(NodeDefinition definition) => _definitions.Add(definition);
    }
}
