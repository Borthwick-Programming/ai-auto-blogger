using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowEngine.Domain.Models
{
    public class VisualMetadata
    {
        public string Icon { get; set; } = "default";
        public string Color { get; set; } = "#CCCCCC";//default is white
        public string Category { get; set; } = "Uncategorized";
    }
}
