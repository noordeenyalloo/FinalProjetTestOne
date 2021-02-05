using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjetTestOne.Models.Dto
{
    public class UpdateProjectDto
    {
        public int Id { get; set; }
        [Required]
        public string ProjectName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string ProjectManagerId { get; set; }
        [Required]
        public string TeamLeaderId { get; set; }
        [Required]
        public List<string> DeveloperIds { get; set; }
    }
}