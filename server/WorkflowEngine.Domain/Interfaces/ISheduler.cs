using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowEngine.Domain.ValueObjects;

namespace WorkflowEngine.Domain.Interfaces
{
    /// <summary>
    /// Represents an abstraction for scheduling future node execution.
    /// Implementations may use Quartz, Hangfire, or in-memory timers.
    /// This belongs in the Domain because the Runtime or Infrastructure
    /// will implement it while the core logic depends only on this interface.
    /// </summary>
    public interface ISheduler
    {
        /// <summary>
        /// Schedule a one-time or recurring job for a specific node.
        /// </summary>
        /// <param name="nodeId">The NodeId to be triggered</param>
        /// <param name="cronExpression">Cron format schedule string</param>
        /// <param name="callback">Delegate to execute when triggered</param>
        void Schedule(NodeId nodeId, string cronExpression, Action callback);

        /// <summary>
        /// Cancel a scheduled job for a given node.
        /// </summary>
        /// <param name="nodeId">The NodeId of the scheduled job to cancel</param>
        void Cancel(NodeId nodeId);
    }
}
