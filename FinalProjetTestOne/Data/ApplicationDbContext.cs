using System;
using System.Collections.Generic;
using System.Text;
using FinalProjetTestOne.Models;
using FinalProjetTestOne.Models.Relations;
using FinalProjetTestOne.Models.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FinalProjetTestOne.Models.User.UserDto;
using FinalProjetTestOne.Models.Dto;

namespace FinalProjetTestOne.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectManager> ProjectManagers { get; set; }
        public virtual DbSet<TeamLeader> TeamLeaders { get; set; }
        public virtual DbSet<Developer> Developers { get; set; }
        public virtual DbSet<Sprint> Sprints { get; set; }
        public virtual DbSet<SprintTask> SprintTasks { get; set; }
        public virtual DbSet<Work> Works { get; set; }
        public virtual DbSet<File> Files { get; set; }
        public virtual DbSet<ProjectDeveloper> ProjectDevelopers { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ProjectDeveloper>().HasKey(x => new { x.DeveloperId, x.ProjectId });

        }
        public DbSet<FinalProjetTestOne.Models.User.UserDto.UpdateUserDto> UpdateUserDto { get; set; }
        public DbSet<FinalProjetTestOne.Models.Dto.UpdateTaskDto> UpdateTaskDto { get; set; }
        public DbSet<FinalProjetTestOne.Models.Dto.UpdateWorkDto> UpdateWorkDto { get; set; }
    }
}
