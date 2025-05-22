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

        /// <summary>
        /// Configures the model for the database context by defining entity relationships, constraints, and indexes.
        /// </summary>
        /// <remarks>This method is called when the model for the context is being created. It is used to
        /// configure entity relationships, such as one-to-many associations, and to define constraints like unique
        /// indexes and cascading delete behaviors.  The method ensures: - A unique index is created on the <see
        /// cref="User.Username"/> property. - A one-to-many relationship between <see cref="Project"/> and <see
        /// cref="User"/> with cascading delete behavior. - A one-to-many relationship between <see
        /// cref="NodeInstance"/> and <see cref="Project"/> with cascading delete behavior.  Override this method to
        /// customize the model configuration further.</remarks>
        /// <param name="modelBuilder">The <see cref="ModelBuilder"/> used to configure the entity framework model.</param>
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
        }
    }
}
