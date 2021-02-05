using FinalProjetTestOne.Models.User;
using FinalProjetTestOne.Models.User.UserDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjetTestOne.Repository.Users
{
    public interface projectManagerRepository
    {
        public Task AddDeveloper(CreateUserDto CreateUserDto);
        public void UpdateDeveloper(UpdateUserDto Developer);
        public void DeleteDeveloper(Developer developer);
        public List<Developer> GetDevelopers();
        public Developer GetDeveloperById(string Id);
    }
}
