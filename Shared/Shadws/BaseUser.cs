using AutoHistoryCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Shadws
{
    public  abstract  class BaseUser : HistoryBaseModel
    {


        [Key]
        public int Id { get; set; }

        [Display(Name = "نام")]
        public string Name { get; set; }

       

        [Display(Name = "شماره موبایل")]
        public string PhoneNubmber { get; set; }

      

      

        [Display(Name = "تاریخ ثبت نام")]
        public DateTime? BeginDate { get; set; }

        [Display(Name = "اجازه فعالیت")]
        public bool AllowActivity { get; set; }


       


    }
}
