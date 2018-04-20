using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bright_Ideas.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Bright_Ideas.Controllers
{
    public class HomeController : Controller
    {
        private Bright_IdeasContext _context;
    
        public HomeController(Bright_IdeasContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register(UserViewModel model, string Email)
        {
            if(ModelState.IsValid)
            {
                User newUser = new User
                {
                    Name = model.Name,
                    Alias = model.Alias,
                    Email = model.Email,
                    Password = model.Password,
                };  

                _context.users.Add(newUser);
                _context.SaveChanges();
                User user2 = _context.users.SingleOrDefault(user => user.Email == Email);
                HttpContext.Session.SetInt32("userId", user2.UserId);
                HttpContext.Session.SetString("userName", user2.Name);
                return RedirectToAction("Dashboard");
            }
            return View("Index");
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(string Email, string Password)
        {

            User user1 = _context.users.SingleOrDefault(user => user.Email == Email);
            if(user1 != null && user1.Password == Password)
            {
                HttpContext.Session.SetString("userName", user1.Name);
                HttpContext.Session.SetInt32("userId", user1.UserId);
                return RedirectToAction("Dashboard");
            }
            return View("Index");
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("bright_ideas")]
        public IActionResult Dashboard()
        {
            if(HttpContext.Session.GetInt32("userId") != null)
            {
            List<Idea> AllIdeas = _context.ideas.OrderByDescending(x => x.Count)
                                        .Include(u => u.Creator)
                                        .Include(u => u.Attendees).ThenInclude(y => y.User)
                                        .ToList();
            @ViewBag.ideas = AllIdeas;
            @ViewBag.user= HttpContext.Session.GetInt32("userId");
            @ViewBag.userName = HttpContext.Session.GetString("userName");
            return View("Dashboard");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("AddIdea")]
        public IActionResult CreateIdea(IdeaViewModel model)
        {
            if(ModelState.IsValid)
            {
                Idea newIdea = new Idea
                {
                    Description = model.Description,
                    CreatorId = HttpContext.Session.GetInt32("userId").Value
                };  
                _context.ideas.Add(newIdea);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        [Route("ShowIdea/{ideaId}")]
        public IActionResult ShowIdea(int ideaId)
        {
            Idea showIdea = _context.ideas.Include(x => x.Attendees).ThenInclude(u => u.User)
                                .Where(wed => wed.IdeaId == ideaId).SingleOrDefault();
            ViewBag.showIdea = showIdea;
            return View("ShowIdea");
        }

        [HttpGet]
        [Route("users/{userId}")]
        public IActionResult ShowUserProfile(int userId)
        {
            User showUser = _context.users.Include(x => x.Invitations).ThenInclude(u => u.User)
                                .Where(wed => wed.UserId == userId).SingleOrDefault();
            List<Idea> userIdeas = _context.ideas.Where(u=>u.CreatorId == userId).ToList();
            int counter = 0;
            foreach(var x in userIdeas){
                counter+=1;
            }
            List<Likes> userLikess = _context.users_ideas.Where(u=>u.UserId == userId).ToList();
            int counter2 = 0;
            foreach(var x in userLikess){
                counter2+=1;
            }
            ViewBag.showCount = counter;
            ViewBag.showLikes = counter2;
            ViewBag.showUser = showUser;
            return View("UserProfile");
        }

        [HttpGet]
        [Route("Like/{ideaId}")]
        public IActionResult AddLike(int ideaId)
        {
            int userId1 = HttpContext.Session.GetInt32("userId").Value;
            Likes chkLike = _context.users_ideas.SingleOrDefault(x => x.IdeaId == ideaId && x.UserId == userId1);
            if(chkLike == null){
            Likes newLike = new Likes
            {
                UserId = userId1,
                IdeaId = ideaId
            };

            _context.users_ideas.Add(newLike);
            Idea RetrievedIdea = _context.ideas.SingleOrDefault(idea => idea.IdeaId == ideaId);
            RetrievedIdea.Count += 1;
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
            }
            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        [Route("delete/{ideaId}")]
        public IActionResult DeleteIdea(int ideaId)
        {   
            Idea RetrievedIdea = _context.ideas.SingleOrDefault(rev => rev.IdeaId == ideaId);
            _context.ideas.Remove(RetrievedIdea);
            List<Likes> RetrievedLikes = _context.users_ideas.Where(x=>x.IdeaId==ideaId).ToList();
            foreach(var x in RetrievedLikes){
                _context.users_ideas.Remove(x);
            }
            _context.SaveChanges();
            return RedirectToAction("Dashboard");

        }
    }
}
