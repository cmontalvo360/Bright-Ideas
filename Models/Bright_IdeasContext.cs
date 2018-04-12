using System;
using Microsoft.EntityFrameworkCore;

namespace Bright_Ideas.Models
{
    public class Bright_IdeasContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public Bright_IdeasContext(DbContextOptions<Bright_IdeasContext> options) : base(options) { }
        public DbSet<Idea> ideas { get; set; }
        public DbSet<User> users {get; set; }
        public DbSet<Likes> users_ideas {get; set; }
    }
}