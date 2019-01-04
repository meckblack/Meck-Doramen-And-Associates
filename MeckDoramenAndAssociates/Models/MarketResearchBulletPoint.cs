using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MeckDoramenAndAssociates.Models
{
    public class MarketResearchBulletPoint : Transport
    {
        #region Data Model

        public int MarketResearchBulletPointId { get; set; }

        [Required(ErrorMessage = "Body is required")]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        #endregion

        #region Foreign Key

        [DisplayName("MarketResearchParagraph")]
        public int MarketResearchParagraphId { get; set; }
        [ForeignKey("MarketResearchParagraphId")]
        public MarketResearchParagraph MarketResearchParagraph { get; set; }

        #endregion
    }
}
