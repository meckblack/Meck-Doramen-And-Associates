using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MeckDoramenAndAssociates.Models
{
    public class Role : Transport
    {
        #region Data Model

        public int RoleId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        
        [Display(Name = "Manage About Us")]
        public bool CanManageAboutUs { get; set; }

        [Display(Name = "Manage Landing Details")]
        public bool CanManageLandingDetails { get; set; }

        [Display(Name = "Manage Market Research")]
        public bool CanManageMarketResearch { get; set; }

        [Display(Name = "Manage Services")]
        public bool CanManageServices { get; set; }

        [Display(Name = "Manage News")]
        public bool CanManageNews { get; set; }

        [Display(Name = "Manage ApplicationUsers")]
        public bool CanMangeUsers { get; set; }

        [Display(Name = "Manage Enquiry")]
        public bool CanManageEnquiry { get; set; }
        
        #endregion
    }
}
