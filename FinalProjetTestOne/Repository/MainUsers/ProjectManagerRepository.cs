using FinalProjetTestOne.Data;
using FinalProjetTestOne.Helpers;
using FinalProjetTestOne.Models;
using FinalProjetTestOne.Models.Dto;
using FinalProjetTestOne.Models.Relations;
using FinalProjetTestOne.Models.User;
using FinalProjetTestOne.Models.User.UserDto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjetTestOne.Repository.MainUsers
{
    public class ProjectManagerRepository :IProjectManagerRepository
    {
        ApplicationDbContext Db;
        MailHandler mail = new MailHandler();
        private UserManager<IdentityUser> UserManager;
        public ProjectManagerRepository(ApplicationDbContext Db, UserManager<IdentityUser> UserManager)
        {
            this.Db = Db;
            this.UserManager = UserManager;
        }
        public List<Project> GetProjects(string Id)
        {
            return Db.Projects.Include(x => x.ProjectManager)
                              .Include(x=> x.Sprints)
                              .Include(x => x.TeamLeader)
                              .Include(x => x.ProjectDevelopers)
                              .ThenInclude(x => x.Developer).ThenInclude(x=> x.Works)
                              .Where(x => x.ProjectManagerId == Id).ToList();
        }
        public void AddProject(CreateProjectDto CreateProjectDto)
        {
            var project = new Project
            {
                ProjectName = CreateProjectDto.ProjectName,
                TeamLeaderId = CreateProjectDto.TeamLeaderId,
                ProjectManagerId = CreateProjectDto.ProjectManagerId,
                Description = CreateProjectDto.Description,
                status = CreateProjectDto.status
            };
            Db.Projects.Add(project);
            Db.SaveChanges();


            foreach (var item in CreateProjectDto.DeveloperIds)
            {
                Db.ProjectDevelopers.Add(new ProjectDeveloper
                {
                    DeveloperId = item,
                    ProjectId = project.Id
                });
                Db.SaveChanges();
            }
            var developers = Db.ProjectDevelopers.Include(x => x.Developer).Where(x => x.ProjectId == project.Id).Select(x=> x.Developer).ToList();
            var teamLeader = Db.TeamLeaders.SingleOrDefault(x => x.Id == CreateProjectDto.TeamLeaderId);
            mail.SendMail(
                                teamLeader.Email,
                                teamLeader.FirstName,
                                "you have been assigned to a new project. For more details, please log in to the system and read the project description.");
            foreach (var dev in developers)
            {
                mail.SendMail(
                                dev.Email,
                                dev.FirstName,
                                "you have been assigned to a new project. For more details, please log in to the system and read the project description.");
            }
        }
        public void UpdateProject(UpdateProjectDto UpdateProjectDto)
        {
            try
            {
                var project = Db.Projects.SingleOrDefault(x => x.Id == UpdateProjectDto.Id);
                project.TeamLeaderId = UpdateProjectDto.TeamLeaderId;
                project.ProjectName = UpdateProjectDto.ProjectName;
                project.Description = UpdateProjectDto.Description;
                var projectDev = Db.ProjectDevelopers.Where(x => x.ProjectId == UpdateProjectDto.Id);
                foreach (var item in projectDev)
                {
                    Db.ProjectDevelopers.Remove(item);
                }
                foreach (var item in UpdateProjectDto.DeveloperIds)
                {
                    Db.ProjectDevelopers.Add(new ProjectDeveloper { DeveloperId = item, ProjectId = project.Id });
                    Db.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void DeleteProject(Project Project)
        {
            var project = Db.Projects.Where(x => x.Id == Project.Id).SingleOrDefault();
            Db.Remove(project);
            Db.SaveChanges();
            var developers = Db.ProjectDevelopers.Where(x => x.ProjectId == Project.Id).ToList();
            foreach (var item in developers)
            {
                Db.ProjectDevelopers.Remove(item);
            }
            Db.SaveChanges();
        }
        public List<Sprint> GetSprints(int Id)
        {
            return Db.Sprints.Include(x => x.Project).Where(x => x.ProjectId == Id).ToList();
        }
        public Project GetProjectById(int Id)
        {
            return Db.Projects.SingleOrDefault(x => x.Id == Id);
        }
        public void ToggleStatus(int Id)
        {
            var project = Db.Projects.SingleOrDefault(x => x.Id == Id);
            var CurentStatus = Db.Projects.Where(x=> x.Id == Id).Select(x => x.status).SingleOrDefault();
            if (CurentStatus == Status.Pendding)
            {
                project.status = Status.Completed;
            }
            else
            {
                project.status = Status.Pendding;
            }
            Db.SaveChanges();
        }
        public List<SprintTask> GetSprintTasks(int Id)
        {
            return Db.SprintTasks.Include(x => x.Developer).Where(x => x.SprintId == Id).ToList();
        }
        public List<Work> GetWorks(int Id, string devId)
        {
            return Db.Works.Include(x => x.Files).Include(x => x.Developer).Where(x => x.SprintTaskId == Id).Where(x=> x.DeveloperId == devId).ToList();
        }
        public Work getWorkById(int Id)
        {
            return Db.Works.SingleOrDefault(x => x.Id == Id);
        }
        public List<File> GetFilesByWorkId(int Id)
        {
            return Db.Files.Where(x => x.WorkId == Id).ToList();
        }
    }
}
