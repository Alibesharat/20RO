﻿using DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.ViewModels
{
    /// <summary>
    /// ویو مدل ثبت نام راننده
    /// </summary>
    public class RegisterDriverViewModel
    {

        [Display(Name = "شماره موبایل")]
        [Required(ErrorMessage = "شماره موبایل الزامی است")]
        [Phone]
        public string PhoneNubmber { get; set; }


        [Display(Name = "نام")]
        [Required(ErrorMessage = "{0}  الزامی است")]
        public string Name { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "{0}  الزامی است")]
        public string LastName { get; set; }

        [Display(Name = "جنسیت")]

        public bool Gender { get; set; }

        [Display(Name = "شهر")]
        [Required(ErrorMessage = "{0}  الزامی است")]
        public int CityId { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "{0}  الزامی است")]
        public string Password { get; set; }

        [Display(Name = "تکرار رمز عبور")]
        [Compare(nameof(Password), ErrorMessage = "رمز عبور و تکرار آن برابر نیستند")]
        public string RePassword { get; set; }


    }

    /// <summary>
    /// ویو مدل ورود راننده
    /// </summary>
    public class LoginDriverViewModel
    {

        [Display(Name = "شماره موبایل")]
        public string PhoneNubmber { get; set; }

        [Display(Name = "رمز عبور")]
        public string Password { get; set; }


    }



    /// <summary>
    /// ویومدل ثبت نام والدین
    /// </summary>
    public class RegisterStudentParrentViewModel
    {
        [Display(Name = "شماره موبایل")]
        [Required(ErrorMessage = "شماره موبایل الزامی است")]
        [Phone]
        public string PhoneNubmber { get; set; }

        [Display(Name = "نام")]
        [Required(ErrorMessage = "{0}  الزامی است")]
        public string Name { get; set; }

        [Display(Name = "نام خانوداگی")]
        [Required(ErrorMessage = "{0}  الزامی است")]
        public string LastName { get; set; }



    }

    /// <summary>
    /// ویو مدل لاگین والدین
    /// </summary>
    public class LoginStudentParrentViewModel
    {
        [Display(Name = "شماره موبایل")]
        public string PhoneNubmber { get; set; }

        [Display(Name = "رمز عبور")]
        public string Password { get; set; }





    }


    /// <summary>
    /// ویو مدل درخواست سرویس
    /// </summary>
    public class RequsetServiceViewModel
    {
        [Display(Name = "والدین")]
        public int StudentParrentId { get; set; }


        [Display(Name = "نام و نام خانوداگی")]
        [Required(ErrorMessage = "نام و نام خانوادگی را وارد نمایید")]
        public string FullName { get; set; }
        

        [Display(Name = "آدرس")]
        [Required(ErrorMessage = "آدرس باید وارد شود")]
        public string Address { get; set; }

        [Display(Name = "یاداشت")]
        public string Note { get; set; }


       
        public int AcademyId { get; set; }



        [Display(Name = "عرض جغرافیایی")]
        public string Latitue { get; set; }

        [Display(Name = "طول جغرافیایی")]
        public string Longtude { get; set; }

        /// <summary>
        /// فاصله در Ui  در نمایش داده نمی شود
        /// </summary>
        public Decimal Distance { get; set; }
    }

    /// <summary>
    /// ویو مدل جزییات سرویس
    /// </summary>



    /// <summary>
    /// ویو مدل  تاریخچه درخواست ها
    /// </summary>
    public class GetServiceHistoryViewModel
    {
        public int ParrentId { get; set; }

        public RequsetSate RequsetSate { get; set; }

    }


    /// <summary>
    /// ویو مدل جزییات درخواست
    /// </summary>
    public class RequsetDetailViewModel
    {
        public int ServiceRequsetId { get; set; }

        public string TrackingCode { get; set; }


    }

    /// <summary>
    /// ویو مدل تغییر وضعیت درخواست 
    /// </summary>
    public class ChangestateViewModel
    {
        /// <summary>
        /// وضعیت اطلاع رسانی
        /// </summary>
        public NotifState NotifState { get; set; }

        public string TaxiCabId { get; set; }

        public string RequseteId { get; set; }

        public int DriverId { get; set; }
    }


    public class AccesptDriverViewModel
    {


        public string TaxiCabId { get; set; }


        public int DriverId { get; set; }
    }

    /// <summary>
    /// مشاهده سوابق سفر
    /// </summary>
    public class GetTaxiCabHistoryViewModel
    {
        public int DriverId { get; set; }

        public  TaxiCabState  TaxiCabState{ get; set; }


    }

    /// <summary>
    /// مشاهده سوابق سفر
    /// </summary>
    public class GetTaxiCabDetailViewModel
    {
        public int DriverId { get; set; }

        public string TaxiCabId { get; set; }

    }


    /// <summary>
    /// ساختار ترانزاکشن
    /// </summary>
    public class TranactionViewModel
    {
        public int UserId { get; set; }
        public int PaynameID { get; set; }
        public int PayTypeID { get; set; }
        public string Cost { get; set; }
        public string TrackingCode { get; set; }
        public bool Success { get; set; }

    }


    public class PayViewModel
    {
        public int ParrentId { get; set; }

        public int RequsetId { get; set; }
    }


    public class GetDetailViewModel
    {
        public string Id { get; set; }
    }

    public class RouteViewModel
    {
        public string Origin { get; set; }
        public string Distination { get; set; }
    }


    public class AcademyFiterViewModel
    {
        public int DistrcitId { get; set; }

        public int AcademyCaregoryId { get; set; }
    }




}
