using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ImaginationStation.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace ImaginationStation.Controllers;

public class PostController : Controller
{
    private MyContext _context;

    public PostController(MyContext context)
    {
        _context = context;
    }

    // ======SHOW ALL POSTS=======
    [LoginCheck]
    [HttpGet("posts")]
    public IActionResult All()
    {
        List<Post> posts = _context.Posts.OrderByDescending(post => post.CreatedAt).Include(post => post.Creator).Include(post => post.PostLikes).ThenInclude(like => like.User).ToList();
        return View("AllPost", posts);
    }

    // =========CREATE POST VIEW=======
    [LoginCheck]
    [HttpGet("posts/new")]
    public IActionResult AddPost()
    {
        return View("AddPost");
    }

     // =========CREATE POST - ACTION====
    [LoginCheck]
    [HttpPost("posts/create")]
    public IActionResult CreatePost(Post newPost)
    {
        if(!ModelState.IsValid)
        {
            return AddPost();
        }

        newPost.UserId = (int)HttpContext.Session.GetInt32("UserId");

        _context.Posts.Add(newPost);
        _context.SaveChanges();

        return Redirect($"/posts/{newPost.PostId}");
    }

    // ==========READ ONE============
    [LoginCheck]
    [HttpGet("posts/{PostId}")]
    public IActionResult ShowOne(int PostId)
    {
        Post posts = _context.Posts.Include(post => post.Creator).Include(pl => pl.PostLikes).ThenInclude(like => like.User).FirstOrDefault(post => post.PostId == PostId);
        if(posts == null)
        {
            return RedirectToAction("All");
        }
        return View("OnePost", posts);
    }

     // ==========UPDATE-VIEW==========
    [LoginCheck]
    [HttpGet("posts/{PostId}/edit")]
    public IActionResult Edit(int PostId)
    {
        Post? post = _context.Posts.FirstOrDefault(post => post.PostId == PostId);

        if(post == null)
        {
            return RedirectToAction("All");
        }

        return View("EditPost", post);
    }

// =======UPDATE - ACTION========
    [LoginCheck]
    [HttpPost("posts/{PostId}/update")]
    public IActionResult UpdatePost(int PostId, Post editedPost)
    {
        if(!ModelState.IsValid)
        {
            return Edit(PostId);
        }

        Post? DBPost = _context.Posts.FirstOrDefault(post => post.PostId == PostId);
        if(DBPost == null)
        {
            return RedirectToAction("All");
        }

        DBPost.Title = editedPost.Title;
        DBPost.Medium = editedPost.Medium;
        DBPost.ImageURL = editedPost.ImageURL;
        DBPost.ForSale = editedPost.ForSale;

        _context.Posts.Update(DBPost);
        _context.SaveChanges();

        return RedirectToAction("ShowOne", new { PostId = PostId});
    }

    // =====DELETE======
    [LoginCheck]
    [HttpPost("posts/{PostId}/delete")]
    public IActionResult DeletePost(int PostId)
    {
        Post? post = _context.Posts.FirstOrDefault(post => post.PostId == PostId);
        if(post != null)
        {
            _context.Posts.Remove(post);
            _context.SaveChanges();
        }

        return RedirectToAction("All");
    }

    // =======LIKE POST - ACTION======
    [LoginCheck]
    [HttpPost("posts/{PostId}/like")]
    public IActionResult Like(int PostId)
    {
        UserPostLike? Like = _context.UserPostLikes.FirstOrDefault(l => l.UserId == HttpContext.Session.GetInt32("UserId") && l.PostId == PostId);

        if(Like != null)
        {
            _context.UserPostLikes.Remove(Like);
        }
        else
        {
            UserPostLike newLike = new UserPostLike()
            {
                PostId = PostId,
                UserId = (int)HttpContext.Session.GetInt32("UserId")
            };
            _context.UserPostLikes.Add(newLike);
        }
        _context.SaveChanges();
        return RedirectToAction("All");
    }

}

public class LoginCheckAttribute : ActionFilterAttribute
{
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        int? userId = context.HttpContext.Session.GetInt32("UserId");

        if (userId == null)
        {
            context.Result = new RedirectToActionResult("Index", "Login", null);
        }
    }
}