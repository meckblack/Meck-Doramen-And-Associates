using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MeckDoramenAndAssociates.Models
{
    public class AboutUsBulletPoint
    {
        #region Data Model

        public int AboutUsBulletPointId { get; set; }

        [Required(ErrorMessage = "Body is required")]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        #endregion

        #region ForiegnKey

        [DisplayName("AboutUsParagraph")]
        public int AboutUsParagraphId { get; set; }
        [ForeignKey("AboutUsParagraphId")]
        public AboutUsParagraph AboutUsParagraph { get; set; }

        #endregion

    }
}
