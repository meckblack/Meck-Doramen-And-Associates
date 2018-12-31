using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MeckDoramenAndAssociates.Models
{
    public class SubService : Transport
    {
        #region Model Data

        public int SubServiceId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        #endregion

        #region Foreign key

        [DisplayName("Service")]
        public int ServiceId { get; set; }
        [ForeignKey("ServiceId")]
        public Service Service { get; set; }

        #endregion
    }
}
