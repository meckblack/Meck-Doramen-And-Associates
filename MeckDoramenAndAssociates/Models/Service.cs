using System.ComponentModel.DataAnnotations;

namespace MeckDoramenAndAssociates.Models
{
    public class Service : Transport
    {
        #region Data Model

        public int ServiceId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        #endregion
    }
}
