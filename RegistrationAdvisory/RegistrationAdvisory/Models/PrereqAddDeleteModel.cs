using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace RegistrationAdvisory.Models
{
    public class PrereqAddDeleteModel
    {
        // [Required]
        [Display(Name = "Student ID")]
        public string studentId { get; set; }

        // [Required]
        [Display(Name = "Course ID")]
        public string courseCat { get; set; }


        public List<SelectListItem> courseCats { get; set; }
    }

   
}
