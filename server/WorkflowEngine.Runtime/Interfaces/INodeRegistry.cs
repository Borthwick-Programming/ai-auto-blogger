using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowEngine.Domain.Models;

namespace WorkflowEngine.Runtime.Interfaces
{
    /// <summary>
    /// Defines the contract for a registry that stores available node definitions.
    /// This interface allows consumers (e.g., the API or execution engine)
    /// to query and register nodes without knowing the storage mechanism.
    /// </summary>
    public interface INodeRegistry
    {
        /// <summary>
        /// Returns all currently registered node definitions.
        /// </summary>
        IEnumerable<NodeDefinition> GetAll();
        /// <summary>
        /// Registers a new node definition into the registry.
        /// </summary>
        /// <param name="definition">The node to add</param>
        void Register(NodeDefinition definition);
    }
}
