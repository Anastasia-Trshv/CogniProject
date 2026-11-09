using Cogni.Contracts.Requests;
using Cogni.Models;
using Cogni.Database.Entities;

namespace Cogni.Abstractions.Services;

public interface IPostService
{
    Task<List<Post>> GetAllUserPosts(int id);
    Task<Post> CreatePost(PostRequest post, int userId);
    //Task<Post> UpdatePost(PostRequest post);


    Task<bool> ToggleReactionAsync(int postId, int userId, string reactionType = "like");

    Task<(int ReactionsCount, bool IsReactedByCurrentUser)> GetReactionInfoAsync(int postId, int userId);

    //Task DeletePost(int id);
    Task<bool> DeletePost(int id, int currentUserId, bool isAdmin);
}

