
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
    public interface ITeamLeaderRepository
    {
        public List<Project> GetProjects(string Id);
        public List<Sprint> GetSprints(int Id);
        public List<ProjectDeveloper> GetDevelopers(int Id);
        public Sprint GetSprintById(int Id);
        public void AddSprint(CreateSprintDto createSprintDto );
        public void UpdateSprint(UpdateSprintDto createSprintDto );
        public void DeleteSprint(int Id);
        public void DeleteSprintTask(SprintTask sprintTask);
        public SprintTask GetSprintTaskById(int id);
        public List<SprintTask> GetSprintTasksBySprintId(int Id);
        public List<SprintTask> GetSprintTasksByDeveloperId(string Id);
        public void UpdateSprintTask(UpdateTaskDto updateTaskDto);
        public void CreateSprintTask(CreateTaskDto createTaskDto);
        public List<Work> GetWorks(string Id, int taskId);
        public Work getWorkById(int Id);
        public List<File> GetFilesByWorkId(int Id);
        public void ToggleSprintStatus(int Id);
        public void ToggleTaskStatus(int Id);
        public void ToggleWorkStatus(int Id);
        public void Reject(RejectDto rejectDto);
        public SprintTask Getsprintbytaskid(int Id);
        public void checkStatus(int Id);
    }
}
