using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowEngine.Domain.Models;

namespace WorkflowEngine.Runtime.Interfaces
{
    public interface INodeRegistry
    {
        IEnumerable<NodeDefinition> GetAll();
        void Register(NodeDefinition definition);
    }
}
