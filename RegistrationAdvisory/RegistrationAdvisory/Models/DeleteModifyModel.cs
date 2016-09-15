using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;


namespace RegistrationAdvisory.Models
{
    public class DeleteModifyModel
    {
       // [Required]
        [Display(Name ="Course ID")]
        public string courseId { get; set; }

       // [Required]
        [Display(Name ="Course Category")]
        public string courseCat { get; set; }


        public List<SelectListItem> courseCats { get; set; }

    }
}