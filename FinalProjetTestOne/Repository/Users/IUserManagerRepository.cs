using FinalProjetTestOne.Models.User;
using FinalProjetTestOne.Models.User.UserDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjetTestOne.Repository.Users
{
    public interface IUserManagerRepository
    {
        public Task AddProjectManager(CreateUserDto CreateUserDto);
        public void UpdateProjectManager(UpdateUserDto ProjectManager);
        public void DeleteProjectManager(ProjectManager ProjectManager);
        public ProjectManager GetProjectManagerById(string Id);
        public List<ProjectManager> GetProjectManagers();
    }
}
