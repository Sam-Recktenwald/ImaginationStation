#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImaginationStation.Models;

public class UserPostLike
{
    [Key]
    public int UserPostLikeId {get; set;}

    public int UserId {get; set;}

    public User? User {get; set;}

    public int PostId {get; set;}

    public Post? Post {get; set;}

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}