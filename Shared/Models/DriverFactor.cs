using AutoHistoryCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL
{
    public class DriverFactor : HistoryBaseModel
    {
        [Key]
        public int Id { get; set; }

        [Display(Name ="شروع")]
        public DateTime? From { get; set; }

        [Display(Name = "پایان")]

        public DateTime? To { get; set; }

        [Display(Name = "تعداد جلسات")]

        public int SeassionCount { get; set; }

        [Display(Name ="سرویس")]
        [ForeignKey(nameof(TaxiCab))]
        public int taxiCabeid { get; set; }
        [Display(Name = "سرویس")]
        public virtual TaxiCab TaxiCab { get; set; }


        [ForeignKey(nameof(ServiceRequset))]
        public int serviceRequsetId { get; set; }
        public virtual ServiceRequset ServiceRequset { get; set; }


        [NotMapped]
        [Display(Name = "فاکتور راننده")]
        public string ClassName { get; }
    }
}
