using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;



namespace RegistrationAdvisory.Models
{
   
    public class CourseModel
    {
        [Required]
        [Display(Name ="Course ID")]
        [StringLength(12, ErrorMessage ="Course ID must be between {2} and {1} characters long!", MinimumLength =7)]
        public string courseId { get; set; }

        [Required]
        [Display(Name = "Course Name")]
        [StringLength(200, ErrorMessage = "Course Name should not be more than 200 characters")]
        public string courseName { get; set; }

        [Required]
        [Display(Name = "Offered Semester")]
        public Semester? offering { get; set; }

        [Required]
        [Display(Name = "Credit")]
        [Range(1, 4, ErrorMessage = "Values for {0} number should be between {1} and {2}")]
        public int credit { get; set; }

        [Required]
        [Display(Name = "Prerequisite ID")]
        [StringLength(12, ErrorMessage = "Prerequisite ID must be between {2} and {1} characters long!", MinimumLength = 4)]
        public string preReqId { get; set; }

        [Required]
        [Display(Name = "Prefered Semester No")]
        [Range(1,8, ErrorMessage ="Values for {0} number should be between {1} and {2}")]
        public int semesterNo { get; set; }

        [Required]
        [Display(Name = "Department")]
        public Dept? department { get; set; }

        [Required]
        [Display(Name ="Course Type")]
        public CourseCategory? coursetype { get; set; }

    }
}