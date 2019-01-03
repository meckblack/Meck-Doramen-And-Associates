using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MeckDoramenAndAssociates.Models
{
    public class AboutUsParagraph
    {
        #region Model Data

        public int AboutUsParagraphId { get; set; }

        [Required(ErrorMessage = "Body is required")]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        #endregion

        #region ForiegnKey

        [DisplayName("AboutUs")]
        public int AboutUsId { get; set; }
        [ForeignKey("AboutUsId")]
        public AboutUs AboutUs { get; set; }

        #endregion
    }
}
