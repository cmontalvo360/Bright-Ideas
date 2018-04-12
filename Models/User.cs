using System.Collections.Generic;

namespace Bright_Ideas.Models
{
    public class User
    {
        public int UserId {get; set; }
        public string Name {get; set; }
        public string Alias {get; set; }
        public string Email {get; set; }
        public string Password {get; set; }
        public List<Likes> Invitations {get; set; }
        public User()
        {
            Invitations = new List<Likes>();
        }
    }
}