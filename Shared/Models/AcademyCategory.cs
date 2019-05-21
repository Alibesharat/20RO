using AutoHistoryCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL
{
    public class AcademyCategory : HistoryBaseModel
    {
        [Key]
        public int Id { get; set; }
        [Display(Name="نام گروه")]
        public string Name { get; set; }

        [NotMapped]
        [Display(Name = "نوع آموزشگاه")]
        public string ClassName { get; }
    }
}