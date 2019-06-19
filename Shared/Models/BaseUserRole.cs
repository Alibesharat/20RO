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
        public BaseUserRole()
        {

        }



        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(AppUser))]
        public int UserId { get; set; }
        public BaseUser AppUser { get; set; }


        [ForeignKey(nameof(Role))]
        public int RolId { get; set; }
        public Role Role { get; set; }
    }
}
