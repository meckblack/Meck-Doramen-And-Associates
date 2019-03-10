using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MeckDoramenAndAssociates.Models
{
    public class Partner : Transport
    {
        public int PartnerId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Image { get; set; }

        [Required]
        [DataType(DataType.Url)]
        public string Url { get; set; }
    }
}
