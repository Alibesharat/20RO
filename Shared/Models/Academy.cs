using DAL.Shadws;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL
{
    public class Academy 
    {
        public string Name { get; set; }


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

      
        [Display(Name = "تلفن پشتیبان")]
        public string SupportNumber { get; set; }

        [Display(Name = "پیمانکار")]
        [ForeignKey(nameof(Contractor))]
        public int ContractorId { get; set; }
        [Display(Name = "پیمانکار")]
        public virtual Contractor Contractor { get; set; }

        [NotMapped]
        [Display(Name = "مدرسه")]
        public string ClassName { get; }

        [Display(Name = "ضریب مدرسه")]
        public int AcademyPercent { get; set; }
        




        [JsonIgnore]
        public virtual ICollection<ServiceRequset> ServiceRequsets { get; set; }

       
    }
}
