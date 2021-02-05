using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjetTestOne.Models.Dto
{
    public class RejectDto
    {
        public int Id { get; set; }
        [Required]
        public string RecjectionNote { get; set; }
    }
}
