using FinalProjetTestOne.Helpers;
using FinalProjetTestOne.Models.Relations;
using FinalProjetTestOne.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjetTestOne.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public Status status { get; set; }
        public TeamLeader TeamLeader { get; set; }
        public string TeamLeaderId { get; set; }
        public ProjectManager ProjectManager { get; set; }
        public string ProjectManagerId { get; set; }
        public List<ProjectDeveloper> ProjectDevelopers { get; set; }
        public List<Sprint> Sprints { get; set; }
    }
}