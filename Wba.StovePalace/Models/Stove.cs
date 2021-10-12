using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wba.StovePalace.Models
{
    public class Stove
    {
        public int Id { get; set; }

        [ForeignKey("Brand")]
        public int BrandId { get; set; }
        [Display(Name = "Merk")]
        public Brand Brand { get; set; }

        [ForeignKey("Fuel")]
        public int FuelId { get; set; }
        [Display(Name ="Brandstof")]
        public Fuel Fuel { get; set; }

        [Display(Name ="Modelnaam")]
        [Required(ErrorMessage ="Geef een model(naam) op !")]
        [StringLength(30, ErrorMessage = "Modelnaam maximaal 30 letters")]
        public string ModelName { get; set; }

        [Display(Name ="Vermogen")]
        [DisplayFormat(DataFormatString ="{0:#,##0} W", ApplyFormatInEditMode =false)]
        [Range(500, 20000, ErrorMessage = "Kies een waarde tussen 200 en 20.000")]
        public int Performance { get; set; }

        [Display(Name = "Prijs")]
        [DisplayFormat(DataFormatString = "€ {0:#,##0.00}", ApplyFormatInEditMode = false)]
        [Range(1, 20000, ErrorMessage = "Kies een waarde tussen 1 en 20.000")]
        [Column(TypeName = "decimal(8, 2)")]
        public decimal SalesPrice { get; set; }

        public string ImagePath { get; set; }
    }
}
