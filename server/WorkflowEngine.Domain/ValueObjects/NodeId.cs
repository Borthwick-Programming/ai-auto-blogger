using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowEngine.Domain.ValueObjects
{
    /// <summary>
    /// Represents a strongly-typed wrapper around a node identifier.
    /// Used instead of plain strings to improve type safety and clarity.
    /// </summary>
    public readonly struct NodeId : IEquatable<NodeId>
    {
        public string Value { get; }
        public NodeId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Node ID cannot be null or empty.", nameof(value));

            Value = value;
        }
       
        public override string ToString() => Value;

        public bool Equals(NodeId other) => Value == other.Value;

        public override bool Equals(object? obj) => obj is NodeId other && Equals(other);

        public override int GetHashCode() => Value.GetHashCode();

        public static implicit operator string(NodeId id) => id.Value;

        public static explicit operator NodeId(string value) => new NodeId(value);
    }
}
