using AutoHistoryCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL
{
    public class Course : HistoryBaseModel
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = " عنوان دوره ")]
        public string Name { get; set; }
        [Display(Name = "تاریخ شروع ")]
        public DateTime? BeginDate { get; set; }

        [Display(Name = "تاریخ پایان ")]
        public DateTime? EndDateTime { get; set; }
        [Display(Name = "ساعت برگزاری")]
        public string TeacherName { get; set; }

        [Display(Name = "آموزشگاه ")]
        [ForeignKey(nameof(Academy))]
        public int AcademyId { get; set; }
        [Display(Name = "آموزشگاه ")]

        public virtual Academy Academy { get; set; }

        [Display(Name = "درصد ترافیک ")]
        public int trafficPercent { get; set; }

        [Display(Name = "ساعات برگزاری")]
        public virtual ICollection<Section> Sections { get; set; }

        [JsonIgnore]
        public virtual ICollection<ServiceRequset> ServiceRequsets { get; set; }

        [NotMapped]
        [Display(Name = "دوره ")]
        public string ClassName { get; set; }


        public string FullTitle
        {
            get
            {
                return $"{Name} -  {TeacherName}";
            }
        }
    }
}
