using Cogni.Abstractions.Repositories;

namespace Cogni.Database.Repositories
{
    public class PostRepository : IPostRepository
    {
        public Task<Post> CreatePost(Post post)
        {
            throw new NotImplementedException();
        }

        public Task<Post> DeletePost(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Post>> GetAllUserPosts(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Post> UpdatePost(Post post)
        {
            throw new NotImplementedException();
        }
    }
}
