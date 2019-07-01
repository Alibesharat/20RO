using DAL.Shadws;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL
{
    public class Academy :BaseUser
    {

        public Academy()
        {
            ServiceRequsets = new HashSet<ServiceRequset>();
        }

        [Display(Name = "آدرس")]
        public string Address { get; set; }


        [Display(Name = "منطقه")]
        [ForeignKey(nameof(District))]
        public int DistrictId { get; set; }
        [Display(Name = "منطقه")]

        public District District { get; set; }


        [Display(Name = "عرض جغرافیایی")]
        public string Latitude { get; set; }

        [Display(Name = "طول جغرافیایی")]
        public string Longtude { get; set; }

        [NotMapped]
        public string FullGeo
        {
            get
            {
                return $"{Latitude},{Longtude}";
            }
        }


        [Display(Name = "نوع مدرسه")]
        [ForeignKey(nameof(Category))]
        public int AcademyCategoryId { get; set; }
        [Display(Name = "نوع مدرسه")]
        public AcademyCategory Category { get; set; }

      
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
