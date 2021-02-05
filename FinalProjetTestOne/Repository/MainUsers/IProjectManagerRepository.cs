using FinalProjetTestOne.Models;
using FinalProjetTestOne.Models.Dto;
using FinalProjetTestOne.Models.User;
using FinalProjetTestOne.Models.User.UserDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjetTestOne.Repository.MainUsers
{
    public interface IProjectManagerRepository
    {
        public List<Project> GetProjects(string Id);
        public void AddProject(CreateProjectDto project);
        public void UpdateProject(UpdateProjectDto project);
        public List<Sprint> GetSprints(int Id);
        public Project GetProjectById(int Id);
        public void DeleteProject(Project project);
        public void ToggleStatus(int Id);
        public List<Work> GetWorks(int Id, string devId);
        public List<SprintTask> GetSprintTasks(int Id);
        public Work getWorkById(int Id);
        public List<File> GetFilesByWorkId(int Id);
    }
}
