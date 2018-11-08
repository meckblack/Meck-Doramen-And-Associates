using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MeckDoramenAndAssociates.Models
{
    public class LandingAboutUs
    {
        #region Data Model

        public int LandingAboutUsId { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string Header { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        #endregion
    }
}
