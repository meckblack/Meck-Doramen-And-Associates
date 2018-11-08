using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeckDoramenAndAssociates.Models
{
    public class Vision : Transport
    {
        #region Data Model

        public int VisionId { get; set; }

        public string VisionOne { get; set; }

        public int VisionOneRating { get; set; }

        public string VisionTwo { get; set; }

        public int VisionTwoRating { get; set; }

        public string VisionThree { get; set; }

        public int VisionThreeRating { get; set; }

        public string VisionFour { get; set; }

        public int VisionFourRating { get; set; }

        #endregion
    }
}
