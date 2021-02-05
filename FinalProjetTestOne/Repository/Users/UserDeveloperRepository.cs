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
    public class UserDeveloperRepository : IUserDeveloperRepository
    {
        ApplicationDbContext Db;
        private UserManager<IdentityUser> UserManager;
        public UserDeveloperRepository(ApplicationDbContext Db, UserManager<IdentityUser> UserManager)
        {
            this.Db = Db;
            this.UserManager = UserManager;
        }
        public void UpdateDeveloper(UpdateUserDto Developer)
        {
            var developer = Db.Developers.SingleOrDefault(x => x.Id == Developer.Id);
            developer.FirstName = Developer.FirstName;
            developer.LastName = Developer.LastName;
            developer.Age = Developer.Age;
            developer.UserName = Developer.UserName;
            developer.Email = Developer.Email;
            Db.SaveChanges();
        }
        public async Task AddDeveloper(CreateUserDto CreateUserDto)
        {
            try
            {
                var developer = new Developer
                {
                    FirstName = CreateUserDto.FirstName,
                    LastName = CreateUserDto.LastName,
                    Age = CreateUserDto.Age,
                    UserName = CreateUserDto.UserName,
                    Email = CreateUserDto.Email,
                    EmailConfirmed = true
                };
                Db.Developers.Add(developer);
                var result = await UserManager.CreateAsync(developer, CreateUserDto.Password);
                if (result.Succeeded)
                {
                    Db.UserRoles.Add(new IdentityUserRole<string>
                    {
                        UserId = developer.Id,
                        RoleId = "4"
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
        public void DeleteDeveloper(Developer Developer)
        {
            Db.Developers.Remove(Developer);
            var prjectDevelopers = Db.ProjectDevelopers.Where(x => x.DeveloperId == Developer.Id).ToList();

            foreach (var item in prjectDevelopers)
            {
                Db.ProjectDevelopers.Remove(item);
            }
            Db.SaveChanges();
        }
        public Developer GetDeveloperById(string Id)
        {
            return Db.Developers.SingleOrDefault(x => x.Id == Id);
        }

        public List<Developer> GetDevelopers()
        {
            return Db.Developers.ToList();
        }
    }
}
