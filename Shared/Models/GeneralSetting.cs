using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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

        [Display(Name ="تعداد جلسات(رفت)")]
        public int SeasionCount { get; set; }

        
    }
}
