using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MeckDoramenAndAssociates.Models
{
    public class News: Transport
    {
        public int NewsId { get; set; }

        [Required(ErrorMessage ="Title field is required")]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage ="Body field is required")]
        public string Body { get; set; }

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "This field is required")]
        public string Body1 { get; set; }

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "This field is required")]
        public string Body2 { get; set; }

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "This field is required")]
        public string Body3 { get; set; }

        [Required(ErrorMessage = "Image field is required")]
        public string Image { get; set; }

        [DataType(DataType.Url)]
        [Required(ErrorMessage ="Read More field is required")]
        public string ReadMore { get; set; }
    }
}
