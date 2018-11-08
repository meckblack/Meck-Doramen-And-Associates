using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MeckDoramenAndAssociates.Models
{
    public class Transport
    {
        #region Data Model

        [DisplayName("Created By")]
        public int CreatedBy { get; set; }

        [DisplayName("Date Created")]
        public DateTime DateCreated { get; set; }

        [DisplayName("Date Last Modified")]
        public DateTime DateLastModified { get; set; }

        [DisplayName("Last Modified By")]
        public int LastModifiedBy { get; set; }

        #endregion
    }
}
