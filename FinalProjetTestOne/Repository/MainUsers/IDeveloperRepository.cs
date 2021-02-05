using FinalProjetTestOne.Models;
using FinalProjetTestOne.Models.Dto;
using FinalProjetTestOne.Models.Relations;
using FinalProjetTestOne.Models.User;
using FinalProjetTestOne.Models.User.UserDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjetTestOne.Repository.MainUsers
{
    public interface IDeveloperRepository
    {
        public List<Developer> GetDeveloperProjects(string Id);
        public List<ProjectDeveloper> GetProjects(string Id);
        public List<Sprint> GetSprints(int Id);
        public List<SprintTask> GetTasksBySprintId(int Id);
        public List<SprintTask> GetTasksByDeveloperId(string Id);
        public void AddWork(CreateWorkDto createWorkDto);
        public void UpdateWork(UpdateWorkDto updateWorkDto);
        public void DeleteWork(Work work);
        public void AddWorkAPI(CreateWorkApiDto createWork);
        public List<Work> GetWorks(string UserId, int TaskId);
        public Work GetWorkById(int Id);
        public void checkStatus(string Id);
        public List<File> GetFilesByWorkId(int Id);
    }
}
