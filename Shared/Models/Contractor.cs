using DAL.Shadws;
using System;
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
            Academies = new HashSet<Academy>();
            Drivers = new HashSet<Driver>();
        }


        public virtual ICollection<Academy> Academies { get; set; }
        public virtual ICollection<Driver> Drivers { get; set; }


        [Display(Name = "شناسه ملی شرکت")]
        public string NIdCode { get; set; }

        [Display(Name ="شماره ثبت شرکت")]
        public string RegisterCompanyNumber { get; set; }

        [Display(Name = "تاریخ شروع قرارداد")]
        public DateTime? RelaseDate { get; set; }

    
        [Display(Name = "تاریخ اتمام قرارداد")]
        public DateTime? ExpireDate { get; set; }

        [NotMapped]
        [Display(Name = "پیمانکار ")]
        public string ClassName { get; set; }




    }
}
