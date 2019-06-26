using System.ComponentModel.DataAnnotations;

namespace DAL
{
    /// <summary>
    /// نوع سرویس
    /// </summary>
    public enum ServiceType
    {
        [Display(Name = "سواری")]
        taxi,
        [Display(Name = "ون")]
        van
    }

    /// <summary>
    /// نوع پرداخت
    /// </summary>
    public enum PayType
    {
        [Display(Name = "دستی")]
        handel,
        [Display(Name = "کارت به کارت")]
        CardToCard,
        [Display(Name = "شبا")]
        Sheba,
        [Display(Name = "چک")]
        Check
    }


    /// <summary>
    /// وضعیت درخواست 
    /// </summary>
    public enum RequsetSate
    {
        [Display(Name = "در انتظار تعیین وضعیت مدرسه")]
        AwaitingAcademy,
        [Display(Name = "در انتظار تعیین وضعیت پیمانکار")]
        AwaitingContractor,
        [Display(Name = " لغو ")]
        Cancel,
        [Display(Name = " در حال سرویس دهی ")]
        Servicing,
        [Display(Name = " معلق ")]
        Suspended,
        [Display(Name = " رزرو ")]
        Reserve


    }


    /// <summary>
    /// وضعیت اطلاع رسانی راننده برای سرویس
    /// </summary>
    public enum NotifState
    {

        [Display(Name = "سوار شد")]
        GetOn,
        [Display(Name = "پیاده شد")]
        GetOff


    }

    /// <summary>
    /// وضعیت تاکسی سرویس
    /// </summary>
    public enum TaxiCabState
    {
        /// <summary>
        /// معلق
        /// </summary>
        [Display(Name = "معلق")]
        wait,

        /// <summary>
        /// آماده به کار
        /// </summary>
        [Display(Name = "آماده به کار")]
        Ready,


    }

    /// <summary>
    /// نقشه ها
    /// </summary>
    public enum RolName
    {
        Admin,
        Contractor,
        Parrent,
        Driver,
        academy
    }

}
