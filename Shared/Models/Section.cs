using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL
{
    public class Section
    {
        [Key]
        public int Id { get; set; }

        [Display(Name ="روز هفته")]
        public Weekday weekday { get; set; }

        [Display(Name = "ساعت شروع")]
        public int StartHour { get; set; }
        [Display(Name = "دقیقه شروع")]
        public int StartMiniute { get; set; }

        [Display(Name = "ساعت پایان")]
        public int EndHour { get; set; }
        [Display(Name = "دقیقه پایان")]
        public int EndMiniute { get; set; }

        [Display(Name = "دوره")]
        [ForeignKey(nameof(Course))]
        public int CoursId { get; set; }
        [Display(Name = "دوره")]
        public virtual Course Course { get; set; }


        [NotMapped]
        [Display(Name ="ساعت برگزاری")]
        public string ClassName { get; }

    }


   
}
