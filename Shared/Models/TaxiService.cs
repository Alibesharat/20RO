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
   
   
    public class TaxiService : HistoryBaseModel
    {

        public TaxiService()
        {
            Passnegers = new HashSet<ServiceRequset>();
        }

        [Key]
        public string Id { get; set; }

        [Display(Name = "عنوان سرویس")]
        public string Name { get; set; }

      


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

        [Display(Name ="مسافران")]
        public virtual ICollection<ServiceRequset> Passnegers { get; set; }



        [NotMapped]
        [Display(Name = "تاکسی سرویس")]
        public string ClassName { get; }

    }

   
}
