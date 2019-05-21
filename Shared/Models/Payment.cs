using AutoHistoryCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL
{
    public class Payment : HistoryBaseModel
    {
        [Key]
        public int Id { get; set; }

        [Display(Name ="مبلغ ")]
        public string Ammount { get; set; }

        [Display(Name = "سرویس ")]
        [ForeignKey(nameof(ServiceRequset))]
        public int? RequsetServiceId { get; set; }
        [Display(Name = "سرویس ")]
        public ServiceRequset ServiceRequset { get; set; }

        [Display(Name = "پرداخت کننده ")]
        [ForeignKey(nameof(Parent))]
        public int ParrentId { get; set; }
        [Display(Name = "پرداخت کننده ")]
        public StudentParent Parent { get; set; }


        [Display(Name = "کد پیگیری")]
        public string TrackingCode { get; set; }

        [Display(Name = "وضعیت")]
        public bool Success { get; set; }


        [NotMapped]
        [Display(Name = "پرداخت ")]
        public string ClassName { get; }

    }
}
