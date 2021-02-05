using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjetTestOne.Models.Dto
{
    public class UpdateTaskDto
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int SprintId { get; set; }
        [Required]
        public string DeveloperId { get; set; }
    }
}
