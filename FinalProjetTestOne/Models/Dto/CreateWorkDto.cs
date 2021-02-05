using FinalProjetTestOne.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjetTestOne.Models.Dto
{
    public class CreateWorkDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public WorkStatus workStatus { get; set; }
        [Required]
        public int SprintTaskId { get; set; }
        [Required]
        public string DeveloperId { get; set; }
        public List<IFormFile> TheFile { get; set; }
    }
}
