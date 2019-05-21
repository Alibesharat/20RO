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
        [ForeignKey(nameof(studentParent))]
        public int StudentParrentId { get; set; }
        [Display(Name = "والدین")]
        public virtual StudentParent studentParent { get; set; }

        [Display(Name = "نام و نام خانوداگی")]
        public string FullName { get; set; }

        [Display(Name = "تصویر مسافر")]
        public string IamgePath { get; set; }

        [Display(Name = "جنسیت")]
        public bool gender { get; set; }

        [Display(Name = "سن")]
        public int Age { get; set; }

        [Display(Name = "آدرس")]
        public string Address { get; set; }

        [Display(Name = "عرض جغرافیایی")]
        public string latitue { get; set; }

        [Display(Name = "طول جغرافیایی")]
        public string Longtude { get; set; }


        public decimal Distance { get; set; }

        [Display(Name = "یادداشت پشتیبان")]
        public string Note { get; set; }
        //=========

        [Display(Name = "دوره")]
        [ForeignKey(nameof(course))]
        public int CourseId { get; set; }

        [Display(Name = "دوره")]
        public virtual Course course { get; set; }

        [Display(Name = "قیمت")]
        public int price { get; set; }

        [Display(Name = "وضعیت درخواست")]

        public RequsetSate RequsetState { get; set; }

        [Display(Name = "وضعیت اطلاع رسانی راننده")]
        public NotifState NotifState { get; set; }

        [Display(Name = "درخواست سرویس")]
        public string ClassName { get; }


        
        public TaxiCab cabAsFirst { get; set; }

       
        public TaxiCab cabAsSecond { get; set; }

       
        public TaxiCab cabAsThird { get; set; }

       
        public TaxiCab cabAsFourth { get; set; }


        [Display(Name = "پرداخت ها")]
        public ICollection<Payment> payments { get; set; }

        public virtual ICollection<DriverFactor> DriverFactors { get; set; }


        [NotMapped]
        public string FullGeo
        {
            get
            {
                return $"{latitue},{Longtude}";
            }
        }


        [NotMapped]
        public string Origin { get; set; }

        [NotMapped]
        public string Distination { get; set; }





    }



}
