using FinalProjetTestOne.Helpers;
using FinalProjetTestOne.Models.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjetTestOne.Models.Dto
{
    public class CreateProjectDto
    {
        [Required]
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string TeamLeaderId { get; set; }
        [Required]
        public string ProjectManagerId { get; set; }
        [Required]
        public List<string> DeveloperIds { get; set; }
        public Status status { get; set; }

    }
}
