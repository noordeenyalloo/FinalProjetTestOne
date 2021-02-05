using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjetTestOne.Models.User
{
    public class ProjectManager : MainUser
    {
        public List<Project> Projects { get; set; }
    }
}
