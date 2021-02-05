using FinalProjetTestOne.Data;
using FinalProjetTestOne.Models.User;
using FinalProjetTestOne.Models.User.UserDto;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjetTestOne.Repository.Users
{
    public class UserManagerRepository : IUserManagerRepository
    {
        ApplicationDbContext Db;
        private UserManager<IdentityUser> UserManager;
        public UserManagerRepository(ApplicationDbContext Db, UserManager<IdentityUser> UserManager)
        {
            this.Db = Db;
            this.UserManager = UserManager;
        }

        public async Task AddProjectManager(CreateUserDto CreateUserDto)
        {
            try
            {
                var projectManager = new ProjectManager()
                {
                    FirstName = CreateUserDto.FirstName,
                    LastName = CreateUserDto.LastName,
                    Age = CreateUserDto.Age,
                    UserName = CreateUserDto.UserName,
                    Email = CreateUserDto.Email,
                    EmailConfirmed = true
                };

                var result = await UserManager.CreateAsync(projectManager, CreateUserDto.Password);

                if (result.Succeeded)
                {
                    Db.UserRoles.Add(new IdentityUserRole<string>
                    {
                        UserId = projectManager.Id,
                        RoleId = "2"
                    });
                    Db.SaveChanges();
                }
                else
                {
                    var Error = result.Errors;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void DeleteProjectManager(ProjectManager ProjectManager)
        {
            throw new NotImplementedException();
        }

        public ProjectManager GetProjectManagerById(string Id)
        {
            return Db.ProjectManagers.SingleOrDefault(x => x.Id == Id);
        }

        public List<ProjectManager> GetProjectManagers()
        {
            return Db.ProjectManagers.ToList();
        }

        public void UpdateProjectManager(UpdateUserDto ProjectManager)
        {
            var manager = Db.ProjectManagers.Where(x => x.Id == ProjectManager.Id).SingleOrDefault();
            manager.FirstName = ProjectManager.FirstName;
            manager.LastName = ProjectManager.LastName;
            manager.Age = ProjectManager.Age;
            manager.UserName = ProjectManager.UserName;
            manager.EmailConfirmed = true;
            Db.SaveChanges();
        }
    }
}
