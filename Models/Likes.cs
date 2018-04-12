namespace Bright_Ideas.Models
{
    public class Likes
    {
        public int LikesId {get; set; }

        public int UserId {get; set; }
        public User User {get; set; }

        public int IdeaId {get; set; }
        public Idea Idea {get; set; }
    }
}