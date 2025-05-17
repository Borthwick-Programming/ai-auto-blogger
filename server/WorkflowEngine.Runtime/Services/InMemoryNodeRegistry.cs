using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowEngine.Domain.Models;
using WorkflowEngine.Runtime.Interfaces;

namespace WorkflowEngine.Runtime.Services
{
    public class InMemoryNodeRegistry : INodeRegistry
    {

        private readonly List<NodeDefinition> _definitions = new();

        public IEnumerable<NodeDefinition> GetAll() => _definitions;

        public void Register(NodeDefinition definition) => _definitions.Add(definition);
    }
}
