using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowEngine.Domain.Models
{
    public enum ExecutionTrigger
    {
        OnRun,
        EventHook,
        Scheduled
    }
}
