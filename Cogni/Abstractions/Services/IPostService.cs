using Cogni.Contracts.Requests;
using Cogni.Models;
using Cogni.Database.Entities;

namespace Cogni.Abstractions.Services;

public interface IPostService
{
    Task<List<Post>> GetAllUserPosts(int id);
    Task<Post> CreatePost(PostRequest post, int userId);
    //Task<Post> UpdatePost(PostRequest post);
    Task DeletePost(int id);
}

