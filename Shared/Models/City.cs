using AutoHistoryCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL
{

    public class City : HistoryBaseModel
    {
        public City()
        {
            Districts = new HashSet<District>();
        }


        [Key]

        public int Id { get; set; }

        [Display(Name ="نام شهر")]
        public string Name { get; set; }

        [Display(Name = "ضریب شهر")]
        public int CityPercent { get; set; }



        public virtual ICollection<District> Districts { get; set; }
    
       



        [NotMapped]
        [Display(Name = "شهر")]
        public string ClassName { get; }
    }
}
