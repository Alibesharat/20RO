using AutoHistoryCore;
using DAL.Shadws;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL
{
    /// <summary>
    /// جدول واسط کلاس نقش و کاربر ، برای پیاده سازی ارتباط چند به چند
    /// </summary>
    public class BaseUserRole : HistoryBaseModel
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(appUser))]
        public int UserId { get; set; }
        public BaseUser appUser { get; set; }


        [ForeignKey(nameof(role))]
        public int RolId { get; set; }
        public Role role { get; set; }
    }
}
