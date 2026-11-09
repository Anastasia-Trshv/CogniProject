using Cogni.Database.Entities;

namespace Cogni.Abstractions.Repositories
{
    public interface IPostRepository
    {
        Task<List<Post>> GetAllUserPosts(int id);
        Task<Post> CreatePost(Post post);
        Task<Post> UpdatePost(Post post);
        Task<Post?> GetPostById(int id);
        Task DeletePost(int id);
        Task ToggleReactionAsync(int postId, int userId, string reactionType = "like");

        Task<(int ReactionsCount, bool IsReactedByCurrentUser)> GetReactionInfoAsync(int postId, int userId); //получение информации о реакциях на пост

    }
}
