using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MeckDoramenAndAssociates.Models
{
    public class Enquiry
    {
        #region Data Model

        public int EnquiryId { get; set; }

        [Required(ErrorMessage ="Full name is required")]
        public string FullName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Message is required")]
        public string Message { get; set; }

        #endregion
    }
}
