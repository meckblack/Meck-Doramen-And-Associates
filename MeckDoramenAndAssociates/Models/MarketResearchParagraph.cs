using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MeckDoramenAndAssociates.Models
{
    public class MarketResearchParagraph : Transport
    {
        #region Data Model

        public int MarketResearchParagraphId { get; set; }

        [Required(ErrorMessage = "Body field is required")]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        #endregion

        #region Foreign Key

        [DisplayName("MarketResearch")]
        public int MarketResearchId { get; set; }
        [ForeignKey("MarketResearchId")]
        public MarketResearch MarketResearch { get; set; }

        #endregion
    }
}
