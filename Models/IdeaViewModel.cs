using System.ComponentModel.DataAnnotations;
namespace Bright_Ideas.Models
{
    public class IdeaViewModel
    {
        [Required]
        [MinLength(4, ErrorMessage = "Gotta give enter some kind of Idea!")]
        public string Description { get; set; }
    }
}