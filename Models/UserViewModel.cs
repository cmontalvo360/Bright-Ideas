using System.ComponentModel.DataAnnotations;
namespace Bright_Ideas.Models
{
    public class UserViewModel
    {
        [Required]
        [MinLength(2, ErrorMessage = "Must be more than 1 letter")]
        public string Name { get; set; }
        [Required]
        [MinLength(2, ErrorMessage = "Must be more than 1 letter")]
        public string Alias { get; set; }
        [Required]
        [EmailAddress]
        [MinLength(10, ErrorMessage = "Must be more than 4 letters")]
        public string Email { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "Must be more than 7 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirmation must match.")]
        public string PW_Confirm { get; set; }
    }

    public class Login
    {
        [Required]
        [EmailAddress]
        public string Email {get; set;}
        [Required]
        public string Password {get; set;}
    }
}