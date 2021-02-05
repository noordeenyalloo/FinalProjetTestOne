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
    public class TeamLeaderRepository : ITeamLeaderRepository
    {
        MailHandler mail = new MailHandler();
        ApplicationDbContext Db;
        private UserManager<IdentityUser> UserManager;
        public TeamLeaderRepository(ApplicationDbContext Db, UserManager<IdentityUser> UserManager)
        {
            this.Db = Db;
            this.UserManager = UserManager;
        }
        public List<Project> GetProjects(string Id)
        {
            var c = Db.Projects.Include(x => x.ProjectManager)
                               .Include(x=> x.Sprints)
                               .Include(x => x.ProjectDevelopers)
                               .ThenInclude(x => x.Developer)
                               .Where(x => x.TeamLeaderId == Id).ToList();
            return c;
        }
        public List<Sprint> GetSprints(int Id)
        {
            return Db.Sprints.Include(x=> x.SprintTasks).Include(x => x.Project).Where(x => x.ProjectId == Id).ToList();
        }
        public Sprint GetSprintById(int Id)
        {
            var x = Db.Sprints.SingleOrDefault(x => x.Id == Id);
            return Db.Sprints.SingleOrDefault(x => x.Id == Id);
        }
        public SprintTask Getsprintbytaskid(int Id)
        {
            //Db.Sprints.Include(x => x.SprintTasks).ThenInclude(x => x.Id).Where(x => x.Id == Id).SingleOrDefault()
            var x = Db.SprintTasks.Include(x=> x.Sprint).ThenInclude(x=> x.Project).Where(x=> x.Id == Id).SingleOrDefault();
            return x;
        }
        public void AddSprint(CreateSprintDto createSprintDto)
        {
            try
            {
                var sprint = new Sprint
                {
                    Title = createSprintDto.Title,
                    StartDate = createSprintDto.StartDate,
                    EndDate = createSprintDto.EndDate,
                    ProjectId = createSprintDto.ProjectId,
                    TeamLeaderId = createSprintDto.TeamLeaderId,
                    Description = createSprintDto.Description,
                    status = Status.Pendding
                };
                Db.Sprints.Add(sprint);
                Db.SaveChanges();
                var developers = Db.ProjectDevelopers.Include(x => x.Developer).Where(x => x.ProjectId == sprint.ProjectId).Select(x => x.Developer).ToList();
                foreach (var dev in developers)
                {
                    mail.SendMail(
                                     dev.Email,
                                     dev.FirstName,
                                     "A New Sprint has been updated <br/>" +
                                     "Title : " + createSprintDto.Title + "<br/>" +
                                     "Description : " + createSprintDto.Description + "<br/>" +
                                     "Duration <br/>" +
                                     "From : " + createSprintDto.StartDate + "<br/>" +
                                     "To : " + createSprintDto.EndDate + "<br/>" +
                                     "Best luck PMS bot"
                                     );
                }
            }
            catch (Exception)
            {

                throw;
            }


            //mail.SendMail()
        }
        public void UpdateSprint(UpdateSprintDto createSprintDto)
        {
            try
            {
                var sprint = Db.Sprints.Where(x => x.Id == createSprintDto.Id).SingleOrDefault();
                sprint.Title = createSprintDto.Title;
                sprint.Description = createSprintDto.Description;
                sprint.StartDate = createSprintDto.StartDate;
                sprint.EndDate = createSprintDto.EndDate;
                Db.SaveChanges();
                var developers = Db.ProjectDevelopers.Include(x => x.Developer).Where(x => x.ProjectId == sprint.ProjectId).Select(x => x.Developer).ToList();
                foreach (var dev in developers)
                {
                    mail.SendMail(
                                    dev.Email,
                                    dev.FirstName,
                                    "Sprint has been updated <br/>" +
                                    "Title : " + createSprintDto.Title + "<br/>" +
                                    "Description : " + createSprintDto.Description + "<br/>" +
                                    "Duration <br/>" +
                                    "From : " + createSprintDto.StartDate + "<br/>" +
                                    "To : " + createSprintDto.EndDate + "<br/>" +
                                    "Best luck PMS bot"
                                    );
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public void DeleteSprint(int Id)
        {
            var SP = Db.Sprints.SingleOrDefault(x => x.Id == Id);
            Db.Sprints.Remove(SP);
            Db.SaveChanges();
        }
        public void CreateSprintTask(CreateTaskDto createTaskDto)
        {
            try
            {
                var task = new SprintTask
                {
                    Title = createTaskDto.Title,
                    Description = createTaskDto.Description,
                    DeveloperId = createTaskDto.DeveloperId,
                    SprintId = createTaskDto.SprintId,
                    status = Status.Pendding
                };
                Db.SprintTasks.Add(task);
                Db.SaveChanges();
                var developers = Db.Developers.Where(x => x.Id == createTaskDto.DeveloperId).SingleOrDefault();
                mail.SendMail(
                                developers.Email,
                                developers.FirstName,
                                "A New Task has been Created <br/>" +
                                "Title : " + createTaskDto.Title + "<br/>" +
                                "Description : " + createTaskDto.Description + "<br/>" +
                                "Best luck PMS bot"
                                );
            }
            catch (Exception)
            {

                throw;
            }

        }
        public void UpdateSprintTask(UpdateTaskDto updateTaskDto)
        {
            try
            {
                var task = Db.SprintTasks.Where(x => x.Id == updateTaskDto.Id).SingleOrDefault();
                task.Title = updateTaskDto.Title;
                task.Description = updateTaskDto.Description;
                task.DeveloperId = updateTaskDto.DeveloperId;
                Db.SaveChanges();
                var developers = Db.Developers.Where(x => x.Id == updateTaskDto.DeveloperId).SingleOrDefault();
                mail.SendMail(
                                developers.Email,
                                developers.FirstName,
                                "Task has been Created <br/>" +
                                "Title : " + updateTaskDto.Title + "<br/>" +
                                "Description : " + updateTaskDto.Description + "<br/>" +
                                "Best luck PMS bot"
                                );
            }
            catch (Exception)
            {

                throw;
            }

        }
        public List<SprintTask> GetSprintTasksByDeveloperId(string Id)
        {
            try
            {
                return Db.SprintTasks.Where(x => x.DeveloperId == Id).ToList();
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        public List<SprintTask> GetSprintTasksBySprintId(int Id)
        {
            try
            {
                return Db.SprintTasks.Include(x=> x.Developer).Where(x=> x.SprintId == Id).ToList();
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        public SprintTask GetSprintTaskById(int id)
        {
            return Db.SprintTasks.Where(x => x.Id == id).SingleOrDefault();
        }
        public void DeleteSprintTask(SprintTask sprintTask)
        {
            var task = Db.SprintTasks.Where(x => x.Id == sprintTask.Id).SingleOrDefault();
            Db.Remove(task);
            Db.SaveChanges();
        }
        public List<Work> GetWorks(string Id, int taskId)
        {
            return Db.Works.Include(x=> x.Files).Include(x=> x.SprintTask).Include(x=> x.Developer).Where(x=> x.SprintTaskId ==taskId).Where(x => x.DeveloperId == Id).ToList();
        }
        public List<File> GetFilesByWorkId(int Id)
        {
            return Db.Files.Where(x => x.WorkId == Id).ToList();
        }
        public void ToggleSprintStatus(int Id)
        {

            var sprint = Db.Sprints.SingleOrDefault(x => x.Id == Id);
            var CurentStatus = Db.Sprints.Where(x => x.Id == Id).Select(x => x.status).SingleOrDefault();
            if (CurentStatus == Status.Pendding)
            {
                sprint.status = Status.Completed;
            }
            else
            {
                sprint.status = Status.Pendding;
            }
            Db.SaveChanges();
        }
        public List<ProjectDeveloper> GetDevelopers(int Id)
        {
            var developers = Db.ProjectDevelopers.Include(x => x.Project)
                                           .Include(x => x.Developer).Where(x => x.ProjectId == Id).ToList();
            return developers;
        }
        public void ToggleTaskStatus(int Id)
        {
            var task = Db.SprintTasks.SingleOrDefault(x => x.Id == Id);
            var CurentStatus = Db.SprintTasks.Where(x => x.Id == Id).Select(x => x.status).SingleOrDefault();
            if (CurentStatus == Status.Pendding)
            {
                task.status = Status.Completed;
            }
            else
            {
                task.status = Status.Pendding;
            }
            Db.SaveChanges();
        }
        public void ToggleWorkStatus(int Id)
        {
            var work = Db.Works.SingleOrDefault(x => x.Id == Id);
            var CurentStatus = Db.Works.Where(x => x.Id == Id).Select(x => x.workStatus).SingleOrDefault();
            if (CurentStatus == WorkStatus.Pendding)
            {
                work.workStatus = WorkStatus.Approved;
            }
            else
            {
                work.workStatus = WorkStatus.Pendding;
            }
            Db.SaveChanges();
        }
        public Work getWorkById(int Id)
        {
            return Db.Works.SingleOrDefault(x => x.Id == Id);
        }
        public void Reject(RejectDto rejectDto)
        {
            var work = Db.Works.Where(x => x.Id == rejectDto.Id).SingleOrDefault();
            work.workStatus = WorkStatus.Rejected;
            work.RecjectionNote = rejectDto.RecjectionNote;
            Db.SaveChanges();
        }
        public void checkStatus(int Id)
        {
            var sprints = Db.Sprints.Include(x => x.Project)
                                .Where(x => x.ProjectId == Id)
                                .ToList();
            var projectStatusCheck = Db.Sprints.Include(x => x.Project)
                                               .Where(x => x.ProjectId == Id)
                                               .Select(x => x.status)
                                               .ToList();

            var statusList = new List<Status>();
            foreach (var item in sprints)
            {
                if (item.status == Status.Completed)
                {
                    statusList.Add(item.status);
                }
                else
                {
                    break;
                }
            }
            if (statusList.Count() == projectStatusCheck.Count())
            {
                this.CompleteProject(Id);
            }
        }
        private void CompleteProject(int Id)
        {
            var project = Db.Projects.Where(x => x.Id == Id).SingleOrDefault();
            project.status = Status.Completed;
            
            Db.SaveChanges();
        }
    }
}
