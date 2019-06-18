using System.ComponentModel.DataAnnotations;

namespace DAL
{
    /// <summary>
    /// تنظیمات عمومی
    /// فقط یک ردیف در این جدول ساخته شود
    /// </summary>
    public class GeneralSetting
    {
        [Key]
        public int Id { get; set; }

        public string SiteName { get; set; }

        public string LogoPath { get; set; }

        [Display(Name ="(تومان)قمیت پایه برای خودروی سواری")]
        public int TaxiPercent { get; set; }


        [Display(Name = "(تومان)قمیت پایه برای خودروی ون")]
        public int VanPercent { get; set; }


    }
}
