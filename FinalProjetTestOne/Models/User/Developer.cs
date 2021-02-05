using FinalProjetTestOne.Models.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjetTestOne.Models.User
{
    public class Developer : MainUser
    {
        public List<ProjectDeveloper> ProjectDevelopers { get; set; }
        public List<Work> Works { get; set; }
        public List<SprintTask> SprintTasks { get; set; }
    }
}