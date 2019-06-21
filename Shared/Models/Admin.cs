using AutoHistoryCore;
using System.ComponentModel.DataAnnotations;

namespace DAL
{
    public class Admin : HistoryBaseModel
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }

        public string Password { get; set; }

        public bool AllowActivity { get; set; }



    }
}
