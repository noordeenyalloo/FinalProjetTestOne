using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjetTestOne.Models.User
{
    public class TeamLeader : MainUser
    {
        public List<Project> Projects { get; set; }
        public List<Sprint> Sprints { get; set; }
    }
}