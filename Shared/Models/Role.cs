using AutoHistoryCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL
{
    public class Role : HistoryBaseModel
    {
        public Role()
        {
            
        }
        [Key]
        public int Id { get; set; }

        [Display(Name = "عنوان نقش")]
        public string Name { get; set; }

       
    }

}
