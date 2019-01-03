using System.ComponentModel.DataAnnotations;

namespace MeckDoramenAndAssociates.Models
{
    public class Service : Transport
    {
        #region Data Model

        public int ServiceId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Rating field is required")]
        public int Rating { get; set; }

        [DataType(DataType.MultilineText)]
        public string Explanation { get; set; }

        [Required(ErrorMessage = "Image field is required")]
        public string Image { get; set; }

        #endregion
    }
}
