using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ToDoList.Models.Account
{
    public class RegistrationViewModel
    {
        [MaxLength(12)]
        [MinLength(1)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "სახელი")]
        public string FirstName { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        [MaxLength(12)]
        [MinLength(1)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "გვარი")]
        public string LastName { get; set; }

        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        [Display(Name = "მეილი")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        [Required(ErrorMessage = "The {0} field is required.")]
        public string Email { get; set; }

        //[RegularExpression("([a-z]|[A-Z]|[0-9]|[\\W]){4}[a-zA-Z0-9\\W]{3,11}", ErrorMessage = "Invalid password format")]
        [DataType(DataType.Password)]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "პაროლი")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [Compare("Password")]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "გაიმეორეთ პაროლი")]
        public string RepeatPassword { get; set; }

    }
}