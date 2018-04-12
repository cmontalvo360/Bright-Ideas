using System;
using System.Collections.Generic;

namespace Bright_Ideas.Models
{
    public class Idea
    {
        public int IdeaId {get; set; }
        public string Description {get; set; }
        public int Count {get; set;}
        public int CreatorId {get; set; }
        public User Creator { get; set; }
        public List<Likes> Attendees {get; set; }
        public Idea()
        {
            Attendees = new List<Likes>();
        }
    }
}