using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowEngine.Domain.Models
{
    /// <summary>
    /// Represents metadata for visual elements, including icon, color, and category information.
    /// </summary>
    /// <remarks>This class is used to store and manage metadata that defines the appearance and
    /// categorization of visual elements. It includes properties for specifying an icon, a color, and a
    /// category.</remarks>
    public class VisualMetadata
    {
        public string Icon { get; set; } = "default";
        public string Color { get; set; } = "#CCCCCC";//default is white
        public string Category { get; set; } = "Uncategorized";
    }
}
