using AutoHistoryCore;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL
{
    /// <summary>
    /// درخواست  سرویس
    /// </summary>
    /// 
    [JsonObject(IsReference = true)]
    public class ServiceRequset : HistoryBaseModel
    {

        [Key]
        public int Id { get; set; }

        [Display(Name = "کد درخواست")]
        public string RequsetCode { get; set; }

        [Display(Name = "والدین")]
        [ForeignKey(nameof(StudentParent))]
        public int StudentParrentId { get; set; }
        [Display(Name = "والدین")]
        public virtual StudentParent StudentParent { get; set; }

        [Display(Name = " نام و نام خادنوادگی دانش آموز")]
        public string FullName { get; set; }

        [Display(Name = "کد ملی دانش آموز")]
        public string IrIdCod { get; set; }



        [Display(Name = "سن")]
        public int Age { get; set; }

        [Display(Name = "آدرس")]
        public string Address { get; set; }

        [Display(Name = "عرض جغرافیایی")]
        public string Latitue { get; set; }

        [Display(Name = "طول جغرافیایی")]
        public string Longtude { get; set; }


        public decimal Distance { get; set; }

        [Display(Name = "یادداشت پشتیبان")]
        public string Note { get; set; }
        //=========

       
       
        [Display(Name = "قیمت")]
        public int Price { get; set; }

        [Display(Name = "وضعیت درخواست")]

        public RequsetSate RequsetState { get; set; }

        [Display(Name = "وضعیت اطلاع رسانی راننده")]
        public NotifState NotifState { get; set; }

        [Display(Name = "نوع سرویس")]

        public ServiceType ServiceType { get; set; }

      

        
        [Display(Name = "آموزشگاه")]
        [ForeignKey(nameof(Academy))]
        public int? AcademyId { get; set; }
        [Display(Name = "آموزشگاه")]
        public Academy Academy { get; set; }


        [Display(Name = "آموزشگاه")]
        [ForeignKey(nameof(TaxiService))]
        public string TaxiServiceId { get; set; }
        [Display(Name = "آموزشگاه")]
        public TaxiService TaxiService { get; set; }



        [Display(Name = "اطلاعات حسابداری")]
        public virtual ICollection<Accounting> Accountings { get; set; }


        [NotMapped]
        public string FullGeo
        {
            get
            {
                return $"{Latitue},{Longtude}";
            }
        }


        [NotMapped]
        public string Origin { get; set; }

        [NotMapped]
        public string Distination { get; set; }


        [Display(Name = "درخواست سرویس")]
        public string ClassName { get; }

        
    }



    



}
