using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel.DataAnnotations;

namespace RegistrationAdvisory.Models
{
    public class CourseRegistrationModel
    {
        [Required]
        [Display(Name ="Student ID")]
        public string studentId { get; set; }
        [Required]
        [Display(Name = "Course ID")]
        public string courseId { get; set; }
        [Required]
        [Display(Name = "Semester No")]
        public string semesterNo { get; set; }
        
    }
}
