using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class Const
{

    public const string WebSiteName = "بیسترو ";

    public const string kewords = "طراحی وبسایت";

    /// <summary>
    /// آدرس وبسایت
    /// </summary>
   // public const string Apirootpath = "https://localhost:44353";
    public const string Apirootpath = "http://Api.20ro.net";

    public const string StudentparrentPath = "https://web.20ro.net";
    public const string Driverpath = "https://driver.20ro.net";

    public const string CdnRootpath = "https://cdn.ilicar.ir";



    /// <summary>
    /// مسیر پیش فرض آواتار
    /// </summary>
    public const string DefaulAvatarPath = "~/Media/image/Default.png";

    /// <summary>
    /// مسیر پیش فرض تصاویر بندانگشتی
    /// </summary>
    public const string DefaultTumbnailPath = "https://alphacore.com/avatars/default.png";

    #region Messages
    public const string InterntErrorMessag = "ارتباط با سرور میسر نشد";
    public const string PremisionErrorMessag = "لطفا ابتدا وارد شوید";

    #endregion

    #region DriverUrl 

    /// <summary>
    /// ایا راننده وجود دارد
    /// </summary>
    public const string IsExistdriver = Apirootpath + "/api/Driver/IsExistdriver";

    /// <summary>
    ///ثبت نام راننده
    /// </summary>
    public const string Registerdriver = Apirootpath + "/api/Driver/Registerdriver";

    /// <summary>
    /// ورود راننده
    /// </summary>
    public const string Logindriver = Apirootpath + "/api/Driver/Logindriver";


    /// <summary>
    ///تغییر وضعیت توسط راننده  
    /// </summary>
    public const string ChangeState = Apirootpath + "/api/Driver/ChangeState";


    /// <summary>
    ///تایید تاکسی توسط راننده  
    /// </summary>
    public const string AcceptDriver = Apirootpath + "/api/Driver/AcceptService";

    /// <summary>
    /// دریافت تاریخچه تاکسی
    /// </summary>
    public const string GetTaxiCabHistory = Apirootpath + "/api/Driver/GetTaxiCabHistory";


    /// <summary>
    /// دریافت جزییات یک تاکسی سرویس
    /// </summary>
    public const string GetTaxiCabDetail = Apirootpath + "/api/Driver/GetTaxiCabDetail";


    public const string DriverUploadFileApi = CdnRootpath + "/api/bisro/Getdriverpic";
    public const string Driver_20ro_AvatarPath_Relative = "20ro/Dynamics/Drivers";
    public const string Driver_20ro_AvatarPath_full = CdnRootpath + "/" + Driver_20ro_AvatarPath_Relative;


    #endregion


    #region ParrentUrl



    /// <summary>
    /// آیا والدین وجود دارد
    /// </summary>
    public const string IsExistStudentparrent = Apirootpath + "/api/Parent/IsExistStudentparrent";

    /// <summary>
    /// ثبت نام والدین
    /// </summary>
    public const string RegisterStudentParent = Apirootpath + "/api/Parent/RegisterStudentParent"; 
    
    
    
    /// <summary>
    /// ویرایش  والدین
    /// </summary>
    public const string EditStudentParent = Apirootpath + "/api/Parent/EditStudentParent";

    /// <summary>
    /// ورود والدین
    /// </summary>
    public const string LoginStudentParrent = Apirootpath + "/api/Parent/LoginStudentParrent";

    /// <summary>
    /// درخواست سرویس
    /// </summary>
    public const string RequsertService = Apirootpath + "/api/Parent/RequsertService";

    /// <summary>
    /// تاریخچه سرویس
    /// </summary>
    public const string ServiceHistory = Apirootpath + "/api/Parent/ServiceHistory";

    /// <summary>
    /// دریافت یک سرویس 
    /// </summary>
    public const string ServiceDetail = Apirootpath + "/api/Parent/ServiceDetail";

    /// <summary>
    /// پرداخت
    /// </summary>
    public const string pay = Apirootpath + "/api/Parent/pay";

    /// <summary>
    /// تغییر و وضعیت پرداخت
    /// </summary>
    public const string Setpayment = Apirootpath + "/api/Driver/Setpayment";



    /// <summary>
    /// دریافت لیست  مدرسه ها
    /// </summary>
    public const string GetAcademies = Apirootpath + "/api/Parent/GetAcademies";



    /// <summary>
    /// دریافت یک مدرسه
    /// </summary>
    public const string GetAcademy = Apirootpath + "/api/Parent/GetAcademy";



    /// <summary>
    /// دریافت لیست   مناطق
    /// </summary>
    public const string GetDistrcits = Apirootpath + "/api/Parent/GetDistrcits";


    /// <summary>
    /// دریافت لیست   گروه های مدرسه
    /// </summary>
    public const string GetAcademyCategories = Apirootpath + "/api/Parent/GetAcademyCategories";



    /// <summary>
    /// دریافت لیست    فیلتر شده مدرسه ها بر اساس مقطع و منطقه 
    /// </summary>
    public const string GetFiltredAcademeis = Apirootpath + "/api/Parent/GetFiltredAcademeis";


    /// <summary>
    /// لغو یا رزور درخواست 
    /// </summary>
    public const string CancelAndAcceptRequset = Apirootpath + "/api/Parent/CancelAndAcceptRequset";




    #endregion

    public static string Generatetoken()
    {
        return Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
    }

    public static string GeneratRandomNumber()
    {
        Random r = new Random();
        return r.Next(1342, 9895).ToString();

    }
}


/// <summary>
/// روزهای هفته
/// </summary>
public enum Weekday
{
    [Display(Name = "شنبه")]
    Saturday,
    [Display(Name = "یک شنبه")]
    Sunday,
    [Display(Name = "دوشنبه")]
    Monday,
    [Display(Name = "سه شنبه")]
    Tuesday,
    [Display(Name = "چهارشنبه")]
    thursday,
    [Display(Name = "پنج شنبه")]
    Wednesday,
    [Display(Name = "جمعه")]
    Friday,


}






