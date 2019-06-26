using DAL.Shadws;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL
{
    public class StudentParent : BaseUser
    {

        public StudentParent()
        {
            ServiceRequsets = new HashSet<ServiceRequset>();
        }

        [Display(Name = "والدین")]
        public string ClassName { get; set; }

        [Display(Name = "شماره ثابت")]
        public string telNumber { get; set; }



        [Display(Name = "تایید قوانین")]
        public bool AccesptTerms { get; set; }


        public virtual ICollection<ServiceRequset> ServiceRequsets { get; set; }
    }
}
