using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SkateDate.Data;
using SkateDate.Models;
using SkateDate.Models.PostsViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SkateDate.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private ApplicationDbContext context;

        public PostsController(ApplicationDbContext dbContext)
        {
            context = dbContext;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            // If user logged in, get their friendlist, and show "most recent" posts from Friends
            var friends = context.FriendLists.Where(u => u.OwnerID == User.Identity.Name).ToList();
            List<List<Post>> PostsToDisplay = new List<List<Post>>();
            foreach (var friend in friends)
            {
                var friendsposts = context.Posts.Where(p => p.Author == friend.FriendID).ToList();
                PostsToDisplay.Add(friendsposts);
            }

            //Display the users posts as well
            var myposts = context.Posts.Where(p => p.Author == User.Identity.Name).ToList();
            PostsToDisplay.Add(myposts);

            List<Post> PostsToDisplayFormated = new List<Post>();
            foreach (var list in PostsToDisplay)
            {
                foreach (var post in list)
                {
                    PostsToDisplayFormated.Add(post);
                }
            }
            var posts = PostsToDisplayFormated.OrderByDescending(o => o.CreatedDate);
            //.Sort((x, y) => x.OrderDate.CompareTo(y.OrderDate)) SHOULD PROBABLY USE THIS -
            // - DOES NOT MAKE A NEW LIST, IE LESS MEMORY, BETTER PERFORMANCE

            IndexViewModel model = new IndexViewModel()
            {
                Posts = posts
            };
            return View(model);
        }

        public IActionResult MyPosts()
        {
            var posts = context.Posts.Where(p => p.Author == User.Identity.Name).ToList();
            return View(posts);
        }

        public IActionResult NewPost()
        {

            return View(new NewPostViewModel());
        }

        [HttpPost]
        public IActionResult AddPost(NewPostViewModel model)
        {
            Post post = new Post()
            {
                Author = User.Identity.Name,
                Message = model.Message,
                Lat = model.Lat,
                Lng = model.Lng
            };

            context.Posts.Add(post);
            context.SaveChanges();

            return Redirect("/Posts/MyPosts");
        }

        [HttpGet]
        public IActionResult EditPost(int PostID)//some kind of view model as paramater
        {
            var post = context.Posts.Single(p => p.PostID == PostID);
            var user = User.Identity.Name;
            if (user == post.Author)
            {
                EditPostViewModel model = new EditPostViewModel()
                {
                    Post = post
                };
                return View(model);
            }
            return Redirect("/Posts/MyPosts");
        }

        [HttpPost]
        public IActionResult EditPost(EditPostViewModel model)
        {
            context.Posts.Single(p => p.PostID == model.Post.PostID).Message = model.Post.Message; // Overwrites old stuff
            context.Posts.Single(p => p.PostID == model.Post.PostID).Lat = model.Post.Lat;
            context.Posts.Single(p => p.PostID == model.Post.PostID).Lng = model.Post.Lng;
            context.SaveChanges();
            string a = String.Format("/Posts/Post?PostID={0}", model.Post.PostID);
            return Redirect(a);
        }


        //REMOVE A POST -- CHECK THE VIEW THAT DIREDCTS HERE... EVEN THOUGH I BELIEVE THIS WAS ALREADYTESTED...
        public IActionResult RemovePost(int PostID)
        {
            string user = User.Identity.Name;
            var post = context.Posts.Single(p => p.PostID == PostID);
            if (user == post.Author)
            {
                context.Posts.Remove(post);
                context.SaveChanges();
                return Redirect("/Posts/MyPosts");
            }
            return Redirect("/Posts");
        }

        // Display single post
        public IActionResult Post(int PostID)
        {
            var post = context.Posts.Include(c => c.Comments).Single(p => p.PostID == PostID);
            string author = post.Author;
            string currentuser = User.Identity.Name;

            if (currentuser == author || currentuser == context.FriendLists.Single(u =>
                    u.OwnerID == author && u.FriendID == currentuser).FriendID)
            {
                PostViewModel model = new PostViewModel()
                {
                    Post = post,
                    Comments = post.Comments.ToList()
                };
                return View(model);
            }
            return Redirect("/Friend"); // SHOULD Redirect to an error
        }


        public IActionResult UsersPosts(string username)
        {
            var posts = context.Posts.Where(p => p.Author == username).ToList();
            var user = User.Identity.Name;
            if (user == username || user == context.FriendLists.Single(u => u.OwnerID == username && u.FriendID == user).FriendID)
            {
                return View(posts);
            }
            return Redirect("/Friend");
        }

        [HttpPost]
        public IActionResult AddComment(PostViewModel model)
        {
            model.NewComment.Author = User.Identity.Name;
            var comment = model.NewComment;
            comment.PostID = model.Post.PostID;
            context.Posts.Include(c => c.Comments).Single(p => p.PostID == model.Post.PostID).Comments.Add(comment);
            context.SaveChanges();
            return Redirect(String.Format("/Posts/Post?PostID={0}", model.Post.PostID));
        }

        [HttpPost]
        public IActionResult RemoveComment(PostViewModel model)
        {
            var post = context.Posts.Include(c => c.Comments).Single(p => p.PostID == model.Post.PostID);
            var comment = context.Comments.Single(c => c.CommentID == model.NewComment.CommentID);
            if (User.Identity.Name == comment.Author || User.Identity.Name == post.Author)
            {
                post.Comments.Remove(comment);
                context.SaveChanges();
            }
            return Redirect(String.Format("/Posts/Post?PostID={0}", post.PostID));
        }

        public IActionResult EditComment(PostViewModel model)
        {
            var post = context.Posts.Single(p => p.PostID == model.Post.PostID);
            var comment = context.Comments.Single(c => c.CommentID == model.NewComment.CommentID);
            EditPostViewModel newModel = new EditPostViewModel()
            {
                Post = post,
                Comment = comment
            };
            return View(newModel);
        }

        [HttpPost]
        public IActionResult EditComment(EditPostViewModel model)
        {
            context.Comments.Single(c => c.CommentID == model.Comment.CommentID).TheComment = model.Comment.TheComment;
            context.SaveChanges();

            string a = String.Format("/Posts/Post?PostID={0}", model.Post.PostID);
            return Redirect(a);
        }
    }
}
