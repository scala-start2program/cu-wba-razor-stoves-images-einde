using System.ComponentModel.DataAnnotations;

namespace Wba.StovePalace.Models
{
    public class Fuel
    {
        public int Id { get; set; }

        [Display(Name = "Brandstof")]
        [Required(ErrorMessage = "Naam brandstof is vereist")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Minimaal 2, maximaal 30 letters")]
        public string FuelName { get; set; }
    }
}
