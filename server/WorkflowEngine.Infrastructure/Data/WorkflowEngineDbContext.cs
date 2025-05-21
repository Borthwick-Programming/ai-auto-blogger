using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowEngine.Infrastructure.Entities;

namespace WorkflowEngine.Infrastructure.Data
{
    public class WorkflowEngineDbContext : DbContext
    {
        public WorkflowEngineDbContext(DbContextOptions<WorkflowEngineDbContext> options) : base(options)
        {
        }
        public DbSet<Entities.User> Users { get; set; } = default!;
        public DbSet<Entities.Project> Projects { get; set; } = default!;
        public DbSet<Entities.NodeInstance> NodeInstances { get; set; } = default!;
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
