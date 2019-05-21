using AutoHistoryCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

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



        [Display(Name = "استان")]
        [ForeignKey(nameof(Province))]
        public int ProvinceId { get; set; }
        [Display(Name ="استان")]
        public virtual Province Province { get; set; }
        public virtual ICollection<District> Districts { get; set; }
    
       



        [NotMapped]
        [Display(Name = "شهر")]
        public string ClassName { get; }
    }
}
