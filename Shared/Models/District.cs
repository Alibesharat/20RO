﻿using AutoHistoryCore;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL
{
    public class District : HistoryBaseModel
    {
        public District()
        {
            Academies = new HashSet<Academy>();
        }

        [Key]
        public int Id { get; set; }

        [Display(Name = "نام")]
        public string Name { get; set; }

        [Display(Name = "شهر")]
        [ForeignKey(nameof(City))]
        public int CityId { get; set; }
        [Display(Name = "شهر")]
        public virtual City City { get; set; }

        [Display(Name = "لیست مدرسه ها")]
        public virtual ICollection<Academy> Academies { get; set; }

        [Display(Name = "ضریب منطقه")]
        public int DistrictPercent { get; set; }


        [NotMapped]
        [Display(Name = "منطقه")]
        public string ClassName { get; }

    }
}