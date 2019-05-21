using DAL.Shadws;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL
{
    public class StudentParent : BaseUser
    {
        [Display(Name = "والدین")]
        public string ClassName { get; set; }

        [Display(Name = "کد زبان آموز")]
        public string StudentCode { get; set; }



        [Display(Name = "کد  ملی")]
        public string IrIdCod { get; set; }

        [Display(Name = "تایید قوانین")]
        public bool AccesptTerms { get; set; }



        [Display(Name = "آموزشگاه")]
        [ForeignKey(nameof(academy))]
        public int? AcademyId { get; set; }
        [Display(Name = "آموزشگاه")]
        public Academy academy { get; set; }


        [Display(Name = "دوره")]
        [ForeignKey(nameof(course))]
        public int? CourseId { get; set; }
        [Display(Name = "دوره")]
        public Course course { get; set; }

        public virtual ICollection<ServiceRequset> ServiceRequsets { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
