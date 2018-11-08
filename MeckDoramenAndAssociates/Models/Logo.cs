using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MeckDoramenAndAssociates.Models
{
    public class Logo
    {
        public int LogoId { get; set; }

        [Required(ErrorMessage ="This field is required")]
        public string Image { get; set; }
    }
}
