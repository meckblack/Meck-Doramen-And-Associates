using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeckDoramenAndAssociates.Models
{
    public class LandingSkill : Transport
    {
        #region Data Model

        public int LandingSkillId { get; set; }

        public string Header { get; set; }

        public string Body { get; set; }

        #endregion

    }
}
