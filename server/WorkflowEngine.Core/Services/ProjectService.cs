using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowEngine.Core.Interfaces;
using WorkflowEngine.Core.Models;
using WorkflowEngine.Infrastructure.Data;
using WorkflowEngine.Infrastructure.Entities;

namespace WorkflowEngine.Core.Services
{
    public class ProjectService : IProjectService
    {
        private readonly WorkflowEngineDbContext _db;
        public ProjectService(WorkflowEngineDbContext db) => _db = db;

        private async Task<User> GetOrCreateUser(string windowsName)
        {
            var user = await _db.Users.SingleOrDefaultAsync(u => u.Username == windowsName);
            if (user != null) return user;

            user = new User { Id = Guid.NewGuid(), Username = windowsName };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task<IEnumerable<ProjectDto>> ListAsync(string windowsUserName)
        {
            var u = await GetOrCreateUser(windowsUserName);
            return await _db.Projects
                .Where(p => p.OwnerId == u.Id)
                .Select(p => new ProjectDto(p.Id, p.Name))
                .ToListAsync();
        }

        public async Task<ProjectDto?> GetAsync(Guid id, string windowsUserName)
        {
            var u = await GetOrCreateUser(windowsUserName);
            var p = await _db.Projects
                         .Where(x => x.Id == id && x.OwnerId == u.Id)
                         .SingleOrDefaultAsync();
            return p is null ? null : new ProjectDto(p.Id, p.Name);
        }

        public async Task<ProjectDto> CreateAsync(string windowsUserName, CreateProjectRequest req)
        {
            var u = await GetOrCreateUser(windowsUserName);
            var p = new Project { Id = Guid.NewGuid(), Name = req.Name, OwnerId = u.Id };
            _db.Projects.Add(p);
            await _db.SaveChangesAsync();
            return new ProjectDto(p.Id, p.Name);
        }

        public async Task<bool> DeleteAsync(Guid id, string windowsUserName)
        {
            var u = await GetOrCreateUser(windowsUserName);
            var p = await _db.Projects.SingleOrDefaultAsync(x => x.Id == id && x.OwnerId == u.Id);
            if (p == null) return false;

            _db.Projects.Remove(p);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
