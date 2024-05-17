using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Models.Models
{
    public class Qualifications
    {
        public int Id { get; set; }

        [Required]
        public string Course { get; set; }

        [Required]
        public string University { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Year")]
        public DateTime StartYear { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Year")]
        public DateTime EndYear { get; set; }

        [Required]
        [Range(0,100)]
        public decimal Percentage { get; set; }

        // Foreign Key to ApplicationUser
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }



    }
}
