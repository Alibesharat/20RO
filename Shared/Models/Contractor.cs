using DAL.Shadws;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL
{
    /// <summary>
    /// پیمانکار
    /// </summary>
    public class Contractor : BaseUser
    {

        public Contractor()
        {

        }


        public virtual ICollection<Academy> Academy { get; set; }
        public virtual ICollection<Driver> Driver { get; set; }

        public bool IsCenterAdmin { get; set; }


        [NotMapped]
        [Display(Name = "پیمانکار ")]
        public string ClassName { get; set; }

    }
}
