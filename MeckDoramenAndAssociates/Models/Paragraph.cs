﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MeckDoramenAndAssociates.Models
{
    public class Paragraph
    {
        #region Model Data

        public int ParagraphId { get; set; }

        [Required(ErrorMessage = "Body is required")]
        public int Body { get; set; }

        #endregion

        #region Foreign key

        [DisplayName("SubService")]
        public int SubServiceId { get; set; }
        [ForeignKey("SubServiceId")]
        public SubService SubService { get; set; }

        #endregion
    }
}
