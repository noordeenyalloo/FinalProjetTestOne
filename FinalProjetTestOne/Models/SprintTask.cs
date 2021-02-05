using FinalProjetTestOne.Helpers;
using FinalProjetTestOne.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjetTestOne.Models
{
    public class SprintTask
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Status status { get; set; }
        public Sprint Sprint { get; set; }
        public int SprintId { get; set; }
        public List<Work> Works { get; set; }
        public Developer Developer { get; set; }
        public string DeveloperId { get; set; }
    }
}
