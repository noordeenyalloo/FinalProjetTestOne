using FinalProjetTestOne.Models.User;
using FinalProjetTestOne.Models.User.UserDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjetTestOne.Repository.Users
{
    public interface IUserLeaderRepository
    {
        public Task AddTeamLeader(CreateUserDto CreateUserDto);
        public void UpdateTeamLeader(UpdateUserDto updateUserDto);
        public void DeleteTeamLeader(TeamLeader TeamLeader);
        public TeamLeader GetTeamLeaderById(string Id);
        public List<TeamLeader> GetTeamLeaders();
    }
}
