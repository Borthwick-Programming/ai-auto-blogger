using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowEngine.Domain.Models
{
    /// <summary>
    /// Defines when and how a node should be triggered within a workflow.
    /// </summary>
    public enum ExecutionTrigger
    {
        /// <summary>
        /// Triggered when the workflow runs
        /// </summary>
        OnRun,
        /// <summary>
        /// Triggered by an external event or webhook
        /// </summary>
        EventHook,
        /// <summary>
        /// Triggered on a schedule (e.g., cron expression)
        /// </summary>
        Scheduled
    }
}
