using FinalProjetTestOne.Data;
using FinalProjetTestOne.Helpers;
using FinalProjetTestOne.Models;
using FinalProjetTestOne.Models.Dto;
using FinalProjetTestOne.Models.Relations;
using FinalProjetTestOne.Models.User;
using FinalProjetTestOne.Models.User.UserDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FinalProjetTestOne.Repository.MainUsers
{
    public class DeveloperRepository : IDeveloperRepository
    {
        ApplicationDbContext Db;
        MailHandler mail = new MailHandler();
        private UserManager<IdentityUser> UserManager;
        public DeveloperRepository(ApplicationDbContext Db, UserManager<IdentityUser> UserManager)
        {
            this.Db = Db;
            this.UserManager = UserManager;
        }
        public List<Developer> GetDeveloperProjects(string Id)
        {

            //var projects = Db.ProjectDevelopers.Include(x => x.Developer)
            //                            .Include(x => x.Project)
            //                            .Where(x => x.DeveloperId == Id).ToList();
            var x = Db.Developers.Include(x => x.ProjectDevelopers).ThenInclude(x => x.Project).ThenInclude(x=> x.TeamLeader).ToList();
            return Db.Developers.Include(x => x.ProjectDevelopers).ThenInclude(x => x.Project).ThenInclude(x => x.TeamLeader).ToList();
        }
        public List<ProjectDeveloper> GetProjects(string Id)
        {
            var projects = Db.ProjectDevelopers.Where(x => x.DeveloperId == Id).Include(x => x.Developer)
                                               .Include(x => x.Project).ThenInclude(x=>x.ProjectManager)
                                               .Include(x=> x.Project).ThenInclude(x=> x.Sprints)
                                               .Include(x => x.Project).ThenInclude(x=> x.TeamLeader)
                                               .ToList();
            return projects;
        }
        public List<Sprint> GetSprints(int Id)
        {
            return Db.Sprints.Include(x=> x.SprintTasks).Include(x=> x.TeamLeader).Include(x => x.Project).Where(x => x.ProjectId == Id).ToList();
        }

        public List<SprintTask> GetTasksByDeveloperId(string Id)
        {
            return Db.SprintTasks.Include(x=> x.Works).Where(x => x.DeveloperId == Id).ToList();
        }

        public List<SprintTask> GetTasksBySprintId(int Id)
        {
            return Db.SprintTasks.Where(x => x.SprintId == Id)
                                 .Include(x => x.Works)
                                 .Include(x=> x.Developer)
                                 .Include(x=> x.Sprint)
                                 .ThenInclude(x=> x.TeamLeader).ToList();
        }

        public Work GetWorkById(int Id)
        {
            return Db.Works.SingleOrDefault(x => x.Id == Id);
        }
        public List<Work> GetWorks(string UserId, int TaskId)
        {
            //return Db.Works.Include(x => x.Developer)
            //               .Where(x => x.DeveloperId == UserId)
            //               .Where(x=> x.SprintTaskId == TaskId)
            //               .ToList();

            var x = Db.Works.Where(w => w.DeveloperId == UserId && w.SprintTaskId == TaskId).ToList();

            return x;
        }
        public void AddWork(CreateWorkDto createWorkDto)
        {
            try
            {
                var work = new Work()
                {
                    DeveloperId = createWorkDto.DeveloperId,
                    Title = createWorkDto.Title,
                    Description = createWorkDto.Description,
                    SprintTaskId = createWorkDto.SprintTaskId,
                    workStatus = WorkStatus.Pendding,
                };
                Db.Works.Add(work);
                Db.SaveChanges();
                if (createWorkDto.TheFile != null)
                {
                    foreach (var Fl in createWorkDto.TheFile)
                    {
                        var file = new Models.File()
                        {
                            WorkId = work.Id
                        };
                        Stream st = Fl.OpenReadStream();
                        using (BinaryReader br = new BinaryReader(st))
                        {
                            var byteFile = br.ReadBytes((int)st.Length);
                            file.FileName = Fl.FileName;
                            file.FileType = Fl.ContentType;
                            file.FileBytes = byteFile;
                        }
                        Db.Files.Add(file);
                        Db.SaveChanges();
                    }  
                }   
            }
            catch (Exception)
            {
                throw ;
            }
        }
        public void DeleteWork(Work work)
        {
            Db.Works.Remove(work);
            Db.SaveChanges();
        }
        public void UpdateWork(UpdateWorkDto updateWorkDto)
        {
            var work = Db.Works.Where(x => x.Id == updateWorkDto.Id).SingleOrDefault();
            work.Title = updateWorkDto.Title;
            work.Description = updateWorkDto.Description;
            Db.SaveChanges();
        }
        public void checkStatus(string Id)
        {
            var works = Db.Works.Include(x => x.Developer)
                                .Where(x => x.DeveloperId == Id)
                                .ToList();

            var workStatusCheck = Db.Works.Include(x=> x.Developer)
                                          .Where(x => x.DeveloperId == Id)
                                          .Select(x => x.workStatus)
                                          .ToList();

            var statusList = new List<WorkStatus>();

            works.Count();

            foreach (var item in works)
            {
                if (item.workStatus == WorkStatus.Approved)
                {       
                    statusList.Add(item.workStatus);
                }
                else
                {
                    break;
                }
            }

            if (statusList.Count() == workStatusCheck.Count())
            {
                this.ToggleAllStatus(Id);
            }
        }
        public List<Models.File> GetFilesByWorkId(int Id)
        {
            return Db.Files.Where(x => x.WorkId == Id).ToList();
        }
        private void ToggleAllStatus(string Id)
        {
            var works = Db.Works.Include(x => x.Developer).Where(x => x.DeveloperId == Id).ToList();
            foreach (var item in works)
            {
                item.workStatus = WorkStatus.Completed;
                Db.SaveChanges();
            }
        }
        public void AddWorkAPI(CreateWorkApiDto createWork)
        {
            var work = new Work()
            {
                Title = createWork.Title,
                Description = createWork.Description,
                workStatus = WorkStatus.Pendding,
                DeveloperId = createWork.DeveloperId,
                SprintTaskId = createWork.SprintTaskId,
            };
            Db.Works.Add(work);

            Db.SaveChanges();
        }
    }
}
