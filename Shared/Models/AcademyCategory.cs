using AutoHistoryCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL
{
    public class AcademyCategory : HistoryBaseModel
    {
        public AcademyCategory()
        {
            Academies = new HashSet<Academy>();
        }


        [Key]
        public int Id { get; set; }
        [Display(Name = "نام گروه")]
        public string Name { get; set; }


        public virtual ICollection<Academy> Academies { get; set; }

        [NotMapped]
        [Display(Name = "نوع مدرسه")]
        public string ClassName { get; }
    }
}