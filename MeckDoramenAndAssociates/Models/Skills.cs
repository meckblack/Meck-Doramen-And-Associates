using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MeckDoramenAndAssociates.Models
{
    public class Skills
    {
        public int SkillsId { get; set; }

        [Required(ErrorMessage="Name field is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Rating field is required")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Explanation field is required")]
        [DataType(DataType.MultilineText)]
        public string Explanation { get; set; }

        [Required(ErrorMessage = "Image field is required")]
        public string Image { get; set; }
    }
}
