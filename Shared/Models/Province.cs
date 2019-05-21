using AutoHistoryCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL
{
    public class Province : HistoryBaseModel
    {
        public Province()
        {
            Cities = new HashSet<City>();
        }


        [Key]
        public int Id { get; set; }

        [Display(Name = "نام")]
        public string Name { get; set; }


        public virtual ICollection<City> Cities { get; set; }
      


        [NotMapped]
        [Display(Name ="استان")]
        public string ClassName { get; }
    }
}
