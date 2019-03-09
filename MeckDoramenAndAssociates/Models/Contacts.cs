using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MeckDoramenAndAssociates.Models
{
    public class Contacts : Transport
    {
        #region Data Models 

        public int ContactsId { get; set; }
        
        [Required(ErrorMessage = "This field is required")]
        public string Number { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string Number2 { get; set; }
        
        [Required(ErrorMessage = "This field is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string Address { get; set; }

        #endregion
    }
}
