using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjetTestOne.Models
{
    public class File
    {
        public int Id { get; set; }
        public Byte[] FileBytes { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public Work Work { get; set; }
        public int WorkId { get; set; }
    }
}