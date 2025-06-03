using System.Text.Json;
using Json.Schema;

namespace WorkflowEngine.Runtime.Services
{
    public static class SchemaValidator
    {
        /// <summary>
        /// Alows us to ask if a configuration is valid **according to the schema** defined for this node type
        /// </summary>
        /// <param name="schemaJson"></param>
        /// <param name="configJson"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool IsValid(string schemaJson, string configJson, out string? error)
        {
            var schema = JsonSchema.FromText(schemaJson);
            var jsonElement = JsonDocument.Parse(configJson).RootElement;

            var result = schema.Evaluate(jsonElement, new EvaluationOptions { OutputFormat = OutputFormat.List });

            if (!result.IsValid)
            {
                error = string.Join("; ", result.Details
                    .SelectMany(d => d.Errors ?? new Dictionary<string, string>())
                    .Select(e => $"{e.Key}: {e.Value}")
                    );
                return false;
            }

            error = null;
            return true;
        }
    }
}
