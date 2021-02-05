using FinalProjetTestOne.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjetTestOne.Models.Dto
{
    public class CreateSprintDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public string TeamLeaderId { get; set; }
        [Required]
        public int ProjectId { get; set; }
    }
}
