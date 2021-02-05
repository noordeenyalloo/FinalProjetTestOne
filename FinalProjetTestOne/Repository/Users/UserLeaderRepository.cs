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
    public class UserLeaderRepository : IUserLeaderRepository
    {
        ApplicationDbContext Db;
        private UserManager<IdentityUser> UserManager;
        public UserLeaderRepository(ApplicationDbContext Db, UserManager<IdentityUser> UserManager)
        {
            this.Db = Db;
            this.UserManager = UserManager;
        }
        public async Task AddTeamLeader(CreateUserDto CreateUserDto)
        {
            try
            {
                var teamLeader = new TeamLeader
                {
                    FirstName = CreateUserDto.FirstName,
                    LastName = CreateUserDto.LastName,
                    Age = CreateUserDto.Age,
                    UserName = CreateUserDto.UserName,
                    Email = CreateUserDto.Email,
                    EmailConfirmed = true
                };

                Db.TeamLeaders.Add(teamLeader);

                var result = await UserManager.CreateAsync(teamLeader, CreateUserDto.Password);

                if (result.Succeeded)
                {
                    Db.UserRoles.Add(new IdentityUserRole<string>
                    {
                        UserId = teamLeader.Id,
                        RoleId = "3"
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
        public TeamLeader GetTeamLeaderById(string Id)
        {
            return Db.TeamLeaders.SingleOrDefault(x => x.Id == Id);
        }
        public List<TeamLeader> GetTeamLeaders()
        {
            return Db.TeamLeaders.ToList();
        }
        public void UpdateTeamLeader(UpdateUserDto TeamLeader)
        {
            var teamLeader = Db.TeamLeaders.SingleOrDefault(x => x.Id == TeamLeader.Id);
            teamLeader.FirstName = TeamLeader.FirstName;
            teamLeader.LastName = TeamLeader.LastName;
            teamLeader.Age = TeamLeader.Age;
            teamLeader.UserName = TeamLeader.UserName;
            teamLeader.Email = TeamLeader.Email;
            Db.SaveChanges();
        }
        public void DeleteTeamLeader(TeamLeader TeamLeader)
        {
            throw new NotImplementedException();
        }
    }
}
