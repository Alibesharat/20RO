using AutoHistoryCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL
{
    public class Accounting : HistoryBaseModel
    {

        public Accounting()
        {

        }


        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(ServiceRequset))]
        public int ServiceRequsetId { get; set; }
        public virtual ServiceRequset ServiceRequset { get; set; }


        [Display(Name = "نوع پرداخت")]
        public PayType PayType { get; set; }

        [Display(Name = "مبلغ پرداختی")]
        public int Payed { get; set; }


        [Display(Name = "تاریخ پرداخت")]
        public DateTime? PayDate { get; set; }



        [Display(Name = "تاریخ سررسید بعدی")]
        public DateTime? NextPay { get; set; }

        [Display(Name ="کد رهگیری")]
        public string TrackNumber { get; set; }



        [Display(Name ="توضیح اضافه")]
        public string Comment { get; set; }


    }


   
}
