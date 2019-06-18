using DAL.Shadws;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL
{
    public class Driver : BaseUser
    {
        [Display(Name = "نام خودرو")]
        public string CarName { get; set; }

        [Display(Name = "نوع خودرو")]

        public string CarType { get; set; }

        [Display(Name = "رنگ خودرو")]

        public string CarColor { get; set; }

        [Display(Name = "کد ملی")]

        public string IranianIdCode { get; set; }

        [Display(Name = "شماره گواهینامه")]

        public string DrivingLicense { get; set; }

        [Display(Name = "کد راننده")]

        public int DriverCode { get; set; }

        [Display(Name = "شماره پلاک")]

        public string PelakNumber { get; set; }

      



        [Display(Name = "وضعیت تاهل")]
        public bool IsMaried { get; set; }

       
        [Display(Name = "پیمانکار")]
        [ForeignKey(nameof(Contractor))]
        public int ContractorId { get; set; }
        [Display(Name = "پیمانکار")]
        public virtual Contractor Contractor { get; set; }



        [NotMapped]
        [Display(Name ="راننده")]
        public string ClassName { get;  }



    }
}




