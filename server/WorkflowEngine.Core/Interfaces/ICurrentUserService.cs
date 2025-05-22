using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowEngine.Core.Interfaces
{
    public interface ICurrentUserService
    {
        string Username { get; }
    }
}
