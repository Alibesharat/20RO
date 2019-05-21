using AutoHistoryCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace DAL
{
    /// <summary>
    /// تاکسی سرویس
    /// </summary>
   
   
    public class TaxiCab : HistoryBaseModel
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "عنوان سرویس")]
        public string Name { get; set; }

        //[Display(Name = "زمان حضور راننده درب منزل")]
        //public string ArrivedTime { get; set; }

        [InverseProperty(nameof(ServiceRequset.cabAsFirst))]
      
        public virtual ICollection<ServiceRequset> FirstPassnger { get; set; }

        [InverseProperty(nameof(ServiceRequset.cabAsSecond))]
       
        public virtual ICollection<ServiceRequset> SecondPassnger { get; set; }

        [InverseProperty(nameof(ServiceRequset.cabAsThird))]
     
        public virtual ICollection<ServiceRequset> ThirdPassnger { get; set; }

        [InverseProperty(nameof(ServiceRequset.cabAsFourth))]
       
        public virtual ICollection<ServiceRequset> FourthPassnger { get; set; }

        public virtual ICollection<DriverFactor> DriverFactors { get; set; }

        [Display(Name = "تکمیل شده")]
        public bool IsCompelete { get; set; }



        [Display(Name = "راننده")]
        [ForeignKey(nameof(DriverId))]
        public int DriverId { get; set; }
        [Display(Name = "راننده")]
        public virtual Driver Driver { get; set; }

        [Display(Name = "وضعیت")]
        public TaxiCabState TaxiCabState { get; set; }

        [Display(Name = "سهم راننده(درصد)")]
        [Range(maximum:75,minimum:0,ErrorMessage ="سهم راننده حداکثر 75 درصد می باشد")]
        public int DriverPercent { get; set; }


        [NotMapped]
        [Display(Name = "تاکسی سرویس")]
        public string ClassName { get; }

    }

   
}
