using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowEngine.Infrastructure.Entities;

namespace WorkflowEngine.Infrastructure.Data
{
    /// <summary>
    /// Represents the database context for the Workflow Engine, providing access to the application's data models.
    /// </summary>
    /// <remarks>This context is used to interact with the database for managing users, projects, and node
    /// instances. It configures entity relationships, including unique constraints and cascading delete
    /// behaviors.</remarks>
    public class WorkflowEngineDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowEngineDbContext"/> class using the specified options.
        /// </summary>
        /// <remarks>This constructor is typically used by dependency injection to configure and
        /// initialize the database context.</remarks>
        /// <param name="options">The options to configure the database context. This typically includes connection settings and other
        /// database-specific configurations.</param>
        public WorkflowEngineDbContext(DbContextOptions<WorkflowEngineDbContext> options) : base(options)
        {
        }
        public DbSet<Entities.User> Users { get; set; } = default!;
        public DbSet<Entities.Project> Projects { get; set; } = default!;
        public DbSet<Entities.NodeInstance> NodeInstances { get; set; } = default!;
        public DbSet<NodeConnection> NodeConnections { get; set; }

/// <summary>
/// Configures the entity framework model for the context by defining entity relationships, constraints, and indexes.
/// </summary>
/// <remarks>This method is called during the model creation process to define the structure and relationships of
/// the database schema. It configures the following: <list type="bullet"> <item> <description> A unique index on the
/// <see cref="User.Username"/> property to ensure that usernames are unique. </description> </item> <item>
/// <description> A one-to-many relationship between <see cref="Project"/> and <see cref="User"/> where a project is
/// owned by a user. Deleting a user cascades the deletion of their projects. </description> </item> <item>
/// <description> A one-to-many relationship between <see cref="NodeInstance"/> and <see cref="Project"/> where a node
/// instance belongs to a project. Deleting a project cascades the deletion of its node instances. </description>
/// </item> <item> <description> A one-to-many relationship between <see cref="NodeConnection"/> and <see
/// cref="NodeInstance"/> for both outgoing and incoming connections. Deleting a node instance cascades the deletion of
/// its associated connections. </description> </item> </list></remarks>
/// <param name="modelBuilder">An instance of <see cref="ModelBuilder"/> used to configure the model for the database context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<Project>()
                .HasOne(p => p.Owner)
                .WithMany(u => u.Projects)
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NodeInstance>()
                .HasOne(n => n.Project)
                .WithMany(p => p.NodeInstances)
                .HasForeignKey(n => n.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NodeConnection>()
                .HasOne(nc => nc.FromNode)
                .WithMany(n => n.OutgoingConnections)
                .HasForeignKey(nc => nc.FromNodeInstanceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NodeConnection>()
                .HasOne(nc => nc.ToNode)
                .WithMany(n => n.IncomingConnections)
                .HasForeignKey(nc => nc.ToNodeInstanceId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
