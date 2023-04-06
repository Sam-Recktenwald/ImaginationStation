#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImaginationStation.Models;

public class Post
{
    [Key]
    public int PostId {get; set;}

    [Required(ErrorMessage = "Title is required")]
    public string Title {get; set;}

    [Required(ErrorMessage = "Medium is required")]
    public string Medium {get; set;}

    [Required(ErrorMessage = "ImageURL is required")]
    public string ImageURL {get; set;}

    [Required(ErrorMessage = "For Sale is required")]
    [Display(Name = "For Sale")]
    public bool ForSale {get; set;}

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public int UserId {get; set;}

    public User? Creator {get; set;}

    public List<UserPostLike> PostLikes {get; set;} = new List<UserPostLike>();
}