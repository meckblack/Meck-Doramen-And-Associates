using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MeckDoramenAndAssociates.Models
{
    public class Clients
    {
        public int ClientsId { get; set; }

        [Required(ErrorMessage="This field is required")]
        public int Name { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public int Image { get; set; }
        
        [Required]
        []
        public string WriteUp { get; set; }




    }
}
