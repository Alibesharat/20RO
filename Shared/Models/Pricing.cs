using AutoHistoryCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL
{
    /// <summary>
    /// مدل قیمت گذاری
    /// </summary>
    public class Pricing : HistoryBaseModel
    {
        public int Id { get; set; }

        [Display(Name ="نام")]
        public string Name { get; set; }

        [Display(Name = "آموزشگاه")]
        [ForeignKey(nameof(academy))]
        public int? AcademyId { get; set; }
        [Display(Name = "آموزشگاه")]
        public Academy academy { get; set; }


        [Display(Name = "از (Km)")]
        public decimal FormKilometer { get; set; }

        [Display(Name = "تا (Km)")]
        public decimal ToKilometer { get; set; }

        [Display(Name = "قیمت ثابت (تومان)")]
        public int ConstPrice { get; set; }

        [Display(Name = "قیمت به ازای هر کیلومتر (تومان)")]
        public int PricePerKilometer { get; set; }

        [Display(Name = "درصد")]
        public int Comission { get; set; }

        [NotMapped]
        [Display(Name = "قیمت گذاری")]
        public string ClassName { get; }



    }
}
