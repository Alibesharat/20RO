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



        [Display(Name = "تایید قوانین")]
        public bool AccesptTerms { get; set; }


        public virtual ICollection<ServiceRequset> ServiceRequsets { get; set; }
    }
}
