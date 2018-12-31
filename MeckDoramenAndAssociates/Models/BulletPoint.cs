using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MeckDoramenAndAssociates.Models
{
    public class BulletPoint : Transport
    {
        #region Model Data

        public int BulletPointId { get; set; }

        [Required(ErrorMessage = "Body is required")]
        public string Body { get; set; }

        #endregion

        #region Foreign key

        [DisplayName("Paragraph")]
        public int ParagraphId { get; set; }
        [ForeignKey("ParagraphId")]
        public Paragraph Paragraph { get; set; }

        #endregion
    }
}
