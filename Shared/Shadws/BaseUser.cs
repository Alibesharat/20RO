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

        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }

        [Display(Name = "سن")]
        public int Age { get; set; }

        [Display(Name = "جنسیت")]
        public bool Gender { get; set; }

        [Display(Name = "رمز عبور")]
        public string Password { get; set; }

        [Display(Name = "شماره موبایل")]
        public string PhoneNubmber { get; set; }

        [Display(Name = "وضعیت تایید شماره موبایل")]
        public bool IsMobielVerifed { get; set; }

        [Display(Name = "رایانامه")]
        public string Email { get; set; }

        [Display(Name = "وضعیت تایید ایمیل ")]
        public bool IsEmailVerified { get; set; }

        [Display(Name = "آواتار")]
        public string AvatarPath { get; set; }

        [Display(Name = "تاریخ تولد")]
        public DateTime? BirthDay { get; set; }

        [Display(Name = "تاریخ ثبت نام")]
        public DateTime? BeginDate { get; set; }

        [Display(Name = "اجازه فعالیت")]
        public bool AllowActivity { get; set; }


        public string FullName { get
            {
                return Name + " " + LastName;
            }
        }


    }
}
