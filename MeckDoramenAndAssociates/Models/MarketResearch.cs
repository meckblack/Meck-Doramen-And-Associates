using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MeckDoramenAndAssociates.Models
{
    public class MarketResearch : Transport
    {
        #region Model Data

        public int MarketResearchId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        #endregion
    }
}
