using DAL.Shadws;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL
{
    public class Academy : BaseUser
    {
        [Display(Name = "نام مدیر عامل")]
        public string BossName { get; set; }

        [Display(Name = "آدرس")]
        public string Address { get; set; }



        [Display(Name = "منطقه")]
        [ForeignKey(nameof(district))]
        public int districtId { get; set; }
        [Display(Name = "منطقه")]

        public District district { get; set; }


        [Display(Name = "عرض جغرافیایی")]
        public string latitude { get; set; }

        [Display(Name = "طول جغرافیایی")]
        public string Longtude { get; set; }

        [NotMapped]
        public string FullGeo
        {
            get
            {
                return $"{latitude},{Longtude}";
            }
        }


        [Display(Name = "نوع آموزشگاه")]
        [ForeignKey(nameof(category))]
        public int AcademyCategoryId { get; set; }
        [Display(Name = "نوع آموزشگاه")]
        public AcademyCategory category { get; set; }

        [Display(Name = "شماره موبایل ضروری")]
        public string OtherPhoneNumber { get; set; }
        [Display(Name = "تلفن ثابت")]
        public string OfficeNumber { get; set; }

        [Display(Name = "پیمانکار")]
        [ForeignKey(nameof(Contractor))]
        public int ContractorId { get; set; }
        [Display(Name = "پیمانکار")]
        public virtual Contractor Contractor { get; set; }

        [NotMapped]
        [Display(Name = "آموزشگاه")]
        public string ClassName { get; }

        [JsonIgnore]
        public virtual ICollection<Course> Courses { get; set; }

      

        [JsonIgnore]
        public virtual ICollection<StudentParent> StudentParents { get; set; }

        [JsonIgnore]

        public virtual ICollection<Pricing> Pricings { get; set; }
    }
}
