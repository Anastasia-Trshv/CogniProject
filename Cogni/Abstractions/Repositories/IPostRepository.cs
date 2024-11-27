﻿using Cogni.Models;

namespace Cogni.Abstractions.Repositories
{
    public interface IPostRepository
    {
        Task<List<Post>> GetAllUserPosts(int id);
        Task<Post> CreatePost(Post post);
        Task<Post> UpdatePost(PostModel post);
        Task DeletePost(int id);

    }
}
