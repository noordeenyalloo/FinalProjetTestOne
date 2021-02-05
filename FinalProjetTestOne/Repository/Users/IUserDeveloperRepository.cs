using FinalProjetTestOne.Models.User;
using FinalProjetTestOne.Models.User.UserDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjetTestOne.Repository.Users
{
    public interface IUserDeveloperRepository
    {
        public Task AddDeveloper(CreateUserDto CreateUserDto);
        public void UpdateDeveloper(UpdateUserDto updateUserDto);
        public void DeleteDeveloper(Developer TeamLeader);
        public Developer GetDeveloperById(string Id);
        public List<Developer> GetDevelopers();
    }
}
